using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour
{
    void LateUpdate()
    {
        Vector3 cameraPosition = Camera.main.transform.position;
	    transform.position = new Vector3(cameraPosition.x, 0, cameraPosition.z);

        Vector2 scaleXZ = new Vector2(transform.localScale.x, transform.localScale.z);
        scaleXZ *= 20;

        Vector2 positionXZ = new Vector2(cameraPosition.x, cameraPosition.z);

        float additionalScale = cameraPosition.y;
        transform.FindChild("Ground").localScale = new Vector3(32 + additionalScale, 32 + additionalScale, 32 + additionalScale);
        transform.FindChild("Ground").localPosition = new Vector3(0, 0, 125 + additionalScale * 10);

        positionXZ = Vector2.Scale(positionXZ, new Vector2(1 / scaleXZ.x, 1 / scaleXZ.y));

        transform.FindChild("Ground").renderer.material.SetTextureOffset("_GridTex", positionXZ);
	}
}
