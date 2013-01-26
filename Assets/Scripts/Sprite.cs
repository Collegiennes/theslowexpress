using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Sprite : MonoBehaviour
{
	void Update ()
    {
        transform.rotation = Quaternion.LookRotation(-Camera.main.transform.forward);
	}
}
