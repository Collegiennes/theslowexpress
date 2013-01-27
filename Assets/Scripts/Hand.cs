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

    void Start()
    {
        if (!TimeKeeper.Instance.DebugMode)
        {
            TimeKeeper.Instance.PhaseChanged += EnableBasedOnState;
            EnableBasedOnState();
        }
    }

    void EnableBasedOnState()
    {
        var visible = TimeKeeper.Instance.Phase == GamePhase.Grabbing;

        enabled = visible;
        foreach (var r in GetComponentsInChildren<Renderer>())
            r.enabled = visible;
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

        if (Mathf.Sign(direction.x) != Mathf.Sign(Direction) || deadzoned)
        {
            direction = transform.localPosition;
            deadzoned = true;
        }

        if (isFired)
        {
            sinceFired += TimeKeeper.Instance.DeltaTime;
            var destination = firedDirection * GrabRange;

            transform.localPosition = CoolSmooth.ExpoLinear(transform.localPosition, destination, 0.999f, 0.5f,
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

            transform.localPosition = CoolSmooth.ExpoLinear(transform.localPosition, destination, 0.9999f, 10,
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
}
