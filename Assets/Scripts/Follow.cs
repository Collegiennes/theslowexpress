using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour
{
    public Transform target;
    public float distance;

	void LateUpdate ()
	{
	    if (TimeKeeper.Instance.Phase == GamePhase.Grabbing) return;

        Vector3 delta = transform.position - target.position;
        if(delta != Vector3.zero)
        {
            Vector3 targetPos = target.position + delta.normalized * distance;
            transform.position = CoolSmooth.ExpoLinear(
                transform.position, targetPos, 1, 100, TimeKeeper.Instance.DeltaTime);
        }
	}
}
