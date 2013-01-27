using UnityEngine;

class Hand : MonoBehaviour
{
    public int Direction = 0;

    public Material IdleMaterial = null;
    public Material PointMaterial = null;
    public Material GrabMaterial = null;

    void Start()
    {
        TimeKeeper.Instance.PhaseChanged += EnableBasedOnState;
        EnableBasedOnState();
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

        transform.localPosition = CoolSmooth.ExpoLinear(transform.localPosition, direction, 0.99f, 5,
                                                        TimeKeeper.Instance.DeltaTime);
        GetComponentInChildren<Sprite>().up = transform.localPosition;
        GetComponentInChildren<Renderer>().material = deadzoned ? IdleMaterial : PointMaterial;
    }
}
