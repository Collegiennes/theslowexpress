using System.Collections;
using UnityEngine;

class PlayerCollision : MonoBehaviour
{
    float sinceCollided;

    void Start()
    {
        PlayerLevelling.Instance.LevelChanged += OnLevelChanged;
    }

    void Update()
    {
        sinceCollided += Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other || !other.transform || !other.transform.parent || !other.transform.parent.parent)
            return;

        if (sinceCollided < 0.25) return;

        if (other.transform.parent.parent.gameObject.name.StartsWith("Obstacle"))
        {
            sinceCollided = 0;

            StartCoroutine(ScreenShake());
            PlayerLevelling.Instance.Downgrade();
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

    void OnLevelChanged()
    {
        transform.parent.FindChild("Sprite").GetComponentInChildren<Renderer>().material.SetTexture("_MainTex",
            PlayerLevelling.Instance.CurrentWingedTexture);

        Debug.Log("level changed");
    }
}
