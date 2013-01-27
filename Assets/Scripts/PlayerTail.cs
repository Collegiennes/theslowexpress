using UnityEngine;

class PlayerTail : MonoBehaviour
{
    void Start()
    {
        PlayerLevelling.Instance.LevelChanged += OnLevelChanged;
    }
    void OnDestroy()
    {
        PlayerLevelling.Instance.LevelChanged -= OnLevelChanged;
    }

    void OnLevelChanged()
    {
        transform.FindChild("Sprite").GetComponentInChildren<Renderer>().material.SetTexture("_MainTex",
            PlayerLevelling.Instance.CurrentBodyTexture);

        Debug.Log("level changed");
    }
}
