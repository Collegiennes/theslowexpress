using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public float distance;

    public float oldTime;
    public Vector3 oldPosition;
    public Vector3 vel;

    void Start()
    {
        oldTime = TimeKeeper.Instance.GlobalTime;
        oldPosition = transform.position;
    }

    void LateUpdate()
    {
        if (TimeKeeper.Instance.Phase == GamePhase.Grabbing) return;

        if(TimeKeeper.Instance.GlobalTime != oldTime)
        {
            vel = vel * 0.5f + 0.5f * (transform.position - oldPosition) / (TimeKeeper.Instance.GlobalTime - oldTime);
            oldPosition = transform.position;
            oldTime = TimeKeeper.Instance.GlobalTime;
        }

        Transform ct = Camera.main.transform;
        Vector3 targetPos = transform.position + new Vector3(0, 2, -distance);

        ct.position = CoolSmooth.ExpoLinear(ct.position, targetPos,
            0.9999f, 15, TimeKeeper.Instance.DeltaTime);

        ct.rotation = CoolSmooth.ExpoLinear(ct.rotation,
            Quaternion.LookRotation(transform.position + vel * 0.05f - ct.position),
            0.9f, 10, TimeKeeper.Instance.DeltaTime);
    }
}
