using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour
{
    void LateUpdate()
    {
        Vector3 cameraPosition = Camera.main.transform.position;
	    transform.position = new Vector3(cameraPosition.x, 0, cameraPosition.z);
	}
}
