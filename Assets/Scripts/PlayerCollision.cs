using System.Collections;
using UnityEngine;

class PlayerCollision : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (!other || !other.transform || !other.transform.parent || !other.transform.parent.parent)
            return;

        if (other.transform.parent.parent.gameObject.name.StartsWith("Obstacle"))
        {
            StartCoroutine(ScreenShake());
            SendMessageUpwards("Downgrade", SendMessageOptions.DontRequireReceiver);
        }
    }

    IEnumerator ScreenShake()
    {
        float value = 2;
        while (value >= Mathf.Epsilon)
        {
            value = CoolSmooth.ExpoLinear(value, 0, 0.99f, 2.5f, Time.deltaTime);
            Camera.mainCamera.transform.position += Random.insideUnitSphere * value;
            yield return new WaitForEndOfFrame();
        }
    }
}
