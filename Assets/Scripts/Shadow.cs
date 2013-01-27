using UnityEngine;
using System.Collections;

public class Shadow : MonoBehaviour
{
	void LateUpdate()
    {
        transform.position =
            new Vector3(transform.position.x, 0.01f, transform.position.z);
	}
}
