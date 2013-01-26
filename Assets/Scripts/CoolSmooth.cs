using UnityEngine;

public class CoolSmooth
{
    public static Vector3 ExpoLinear(Vector3 current, Vector3 target,
        float exp, float lin, float time)
    {
        float t = Mathf.Pow((1-exp), time);
        current = current * t + target * (1-t);
        current = Vector3.MoveTowards(current, target, lin*time);
        return current;
    }

    public static float ExpoLinear(float current, float target,
        float exp, float lin, float time)
    {
        float t = Mathf.Pow((1-exp), time);
        current = current * t + target * (1-t);
        current = Mathf.MoveTowards(current, target, lin*time);
        return current;
    }

    public static Quaternion ExpoLinear(Quaternion current, Quaternion target,
        float exp, float lin, float time)
    {
        float angle = Quaternion.Angle(current, target);
        if(angle > 0)
        {
            float t = ExpoLinear(0, angle, exp, lin, time) / angle;
            Debug.Log(current + " " + target);
            return Quaternion.Slerp(current, target, t);
        }
        else
        {
            return target;
        }
    }
}
