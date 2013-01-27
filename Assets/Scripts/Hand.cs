using System.Collections;
using UnityEngine;

class Hand : MonoBehaviour
{
    public int Direction = 0;
    public float ReachDuration = 1;
    public float ComeBackDuration = 1;
    public float GrabRange = 5;

    public Material IdleMaterial = null;
    public Material PointMaterial = null;
    public Material GrabMaterial = null;

    bool isFired, isComingBack;
    float sinceFired, sinceComingBack;
    Vector3 firedDirection;
    float sinceIdle;
    float sinceDisabled;

    void Start()
    {
        if (!TimeKeeper.Instance.DebugMode)
        {
            TimeKeeper.Instance.PhaseChanged += () => EnableBasedOnState(true);
            EnableBasedOnState(false);
        }
    }

    void EnableBasedOnState(bool fade)
    {
        var visible = TimeKeeper.Instance.Phase == GamePhase.Grabbing;

        enabled = visible;
        foreach (var r in GetComponentsInChildren<Renderer>())
        {
            if (visible)
            {
                r.material.SetColor("_Color", new Color(1, 1, 1, 1));
                r.enabled = true;
            }
            else
            {
                if (fade)
                    StartCoroutine(FadeAlpha(r));
                else
                    r.enabled = false;
            }
        }

        if (visible)
        {
            transform.localPosition = new Vector3(Direction, 0, 0);
            GetComponentInChildren<Sprite>().up = Vector3.Normalize(transform.localPosition);
        }
    }

    IEnumerator FadeAlpha(Renderer r)
    {
        float alpha = 1;
        while (alpha > Mathf.Epsilon)
        {
            if (enabled) break;

            alpha = CoolSmooth.ExpoLinear(alpha, 0, 0.9f, 1f, Time.deltaTime);
            r.material.SetColor("_Color", new Color(1, 1, 1, alpha));
            yield return new WaitForSeconds(Time.deltaTime);
        }
        r.enabled = enabled;
    }

    Vector2 RemoveDeadzone(Vector2 input, float deadzone, out bool deadzoned)
    {
        float mag = input.magnitude;
        input = input / mag;
        if (mag < deadzone)
        {
            deadzoned = true;
            return Vector2.up;
        }

        mag = (mag - deadzone) / (1 - deadzone);
        deadzoned = false;
        return input * mag;
    }

    void Update()
    {
        var direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        bool deadzoned;
        direction = RemoveDeadzone(direction, 0.15f, out deadzoned);
        direction = direction.normalized;

        if (direction.y < -0.7f)
        {
            direction.y = -0.7f;
            direction.x = 0.7f * Mathf.Sign(direction.x);
            direction = direction.normalized;
        }

        if (Mathf.Sign(direction.x) != Mathf.Sign(Direction) || deadzoned)
        {
            direction = transform.localPosition;
            deadzoned = true;

            sinceIdle += TimeKeeper.Instance.DeltaTime;
            if (sinceIdle > 0.75f)
                direction = CoolSmooth.ExpoLinear(direction, new Vector3(Direction, 0, 0), 0.9f, 1,
                                                  TimeKeeper.Instance.DeltaTime);
        }
        else
            sinceIdle = 0;

        if (isFired)
        {
            sinceFired += TimeKeeper.Instance.DeltaTime;
            var destination = firedDirection * GrabRange;

            transform.localPosition = CoolSmooth.ExpoLinear(transform.localPosition, destination, 0.99f, 5,
                                                            TimeKeeper.Instance.DeltaTime);

            GetComponentInChildren<Sprite>().up = Vector3.Normalize(transform.localPosition);
            GetComponentInChildren<Renderer>().material = GrabMaterial;

            if (sinceFired >= ReachDuration)
            {
                isFired = false;
                isComingBack = true;
                sinceFired = 0;
            }
        }
        else if (isComingBack)
        {
            sinceComingBack += TimeKeeper.Instance.DeltaTime;
            var destination = firedDirection;

            transform.localPosition = CoolSmooth.ExpoLinear(transform.localPosition, destination, 0.9999f, 15,
                                                            TimeKeeper.Instance.DeltaTime);

            GetComponentInChildren<Renderer>().material = IdleMaterial;

            if (sinceComingBack >= ComeBackDuration)
            {
                isComingBack = false;
                sinceComingBack = 0;
            }
        }
        else
        {

            transform.localPosition = CoolSmooth.ExpoLinear(transform.localPosition, direction, 0.99f, 5,
                                                            TimeKeeper.Instance.DeltaTime);
            GetComponentInChildren<Sprite>().up = transform.localPosition;
            GetComponentInChildren<Renderer>().material = deadzoned ? IdleMaterial : PointMaterial;

            if (!deadzoned && !isFired && Input.GetButton("Fire"))
            {
                firedDirection = direction;
                isFired = true;
            }
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (!enabled) return;

        if (isFired && collider.transform.GetComponent<Bubble>() != null)
            Destroy(collider.gameObject);
    }
}
