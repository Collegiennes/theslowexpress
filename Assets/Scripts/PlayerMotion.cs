using UnityEngine;
using System.Collections;

public class PlayerMotion : MonoBehaviour
{
    Vector3 velocity;

	void Start ()
    {

	}

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
        Vector3 input = new Vector2(
            Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        input = RemoveDeadzone(input, 0.1f);
        Vector3 targetVelocity = 40 * input + Vector3.forward * 20;

        float t = Mathf.Pow(0.0003f, Time.deltaTime);
        velocity = velocity * t + targetVelocity * (1-t);
        velocity = Vector3.MoveTowards(velocity, targetVelocity, 10*Time.deltaTime);

        transform.position += velocity * Time.deltaTime;
    }
}
