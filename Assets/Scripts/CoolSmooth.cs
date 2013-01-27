using UnityEngine;

public class CoolSmooth
{
    public static Vector3 ExpoLinear(Vector3 current, Vector3 target,
        float exp, float lin, float time)
    {
        float mag = (current - target).magnitude;
        return current + (target-current) * ExpoLinear(0, 1, exp, lin/mag, time);
    }

    const float dt = 0.001f;
    public static float ExpoLinear(float current, float target,
        float exp, float lin, float totalTime)
    {
        while(totalTime > 0)
        {
            float time;
            if(totalTime > dt)
            {
                time = dt;
                totalTime -= dt;
            }
            else
            {
                time = totalTime;
                totalTime = 0;
            }
            float t = Mathf.Pow((1-exp), time);
            current += (1-t) * (target - current);
            current = Mathf.MoveTowards(current, target, lin*time);
        }

        return current;
    }

    public static Quaternion ExpoLinear(Quaternion current, Quaternion target,
        float exp, float lin, float time)
    {
        float angle = Quaternion.Angle(current, target);
        if(angle > 0)
        {
            float t = ExpoLinear(0, angle, exp, lin, time) / angle;
            return Quaternion.Slerp(current, target, t);
        }
        else
        {
            return target;
        }
    }
}
