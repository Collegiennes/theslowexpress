using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Sprite : MonoBehaviour
{
    public Vector3 up = Vector3.up;

	void Update ()
    {
        transform.rotation =
            Quaternion.LookRotation(-Camera.main.transform.forward, up);
	}
}
