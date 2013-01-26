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
        oldTime = Time.time;
        oldPosition = transform.position;
    }

    void Update()
    {
        if(Time.time != oldTime)
        {
            vel = (transform.position - oldPosition) / (Time.time - oldTime);
            oldPosition = transform.position;
            oldTime = Time.time;
        }

        Transform ct = Camera.main.transform;
        Vector3 targetPos = transform.position + new Vector3(0, 2, -distance);

        ct.position = CoolSmooth.ExpoLinear(ct.position, targetPos,
            0.9995f, 10, Time.deltaTime);

        ct.rotation = CoolSmooth.ExpoLinear(ct.rotation,
            Quaternion.LookRotation(transform.position + vel * 0.05f - ct.position),
            0.9f, 10, Time.deltaTime);
    }
}
