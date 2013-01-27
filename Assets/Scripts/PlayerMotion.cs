using UnityEngine;
using System.Collections;

public class PlayerMotion : MonoBehaviour
{
    Vector3 velocity;
    float top = 10;
    float bottom = 0.10f;

    Vector2 RemoveDeadzone(Vector2 input, float deadzone)
    {
        float mag = input.magnitude;
        input = input / mag;
        if(mag < deadzone)
        {
            return Vector2.zero;
        }
        else
        {
            mag = (mag - deadzone) / (1-deadzone);
            return input * mag;
        }
    }
	
    void Update()
    {
        switch (TimeKeeper.Instance.Phase)
        {
            case GamePhase.Moving:
            case GamePhase.IntroFastMove:
                Vector3 input = new Vector2(
                    Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                if(input.sqrMagnitude > 1) input = input.normalized;
                input = RemoveDeadzone(input, 0.1f);
                if(input.sqrMagnitude > 0) input = input / Mathf.Sqrt(input.magnitude);
                Vector3 targetVelocity = 40 * input + Vector3.forward * 40;
                float headroom = top - transform.position.y;
                targetVelocity.y = Mathf.Min(targetVelocity.y, headroom*5);
                float footroom = transform.position.y - bottom;
                targetVelocity.y = Mathf.Max(targetVelocity.y, -footroom*5);

                velocity = CoolSmooth.ExpoLinear(
                    velocity, targetVelocity, 0.99f, 40, TimeKeeper.Instance.DeltaTime);
                transform.position += velocity * TimeKeeper.Instance.DeltaTime;

                transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, bottom, top), transform.position.z);
                break;

            case GamePhase.Grabbing:
                // TODO
                break;
        }
    }
}
