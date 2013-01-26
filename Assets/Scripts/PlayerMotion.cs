using UnityEngine;
using System.Collections;

public class PlayerMotion : MonoBehaviour
{
    Vector3 velocity;

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
                Vector3 targetVelocity = 40 * input + Vector3.forward * 20;

                velocity = CoolSmooth.ExpoLinear(
                    velocity, targetVelocity, 0.99f, 40, TimeKeeper.Instance.DeltaTime);
                transform.position += velocity * TimeKeeper.Instance.DeltaTime;
                break;

            case GamePhase.Grabbing:
                // TODO
                break;
        }
    }
}
