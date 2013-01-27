using UnityEngine;


class Inverting : MonoBehaviour
{
    void Start()
    {
        TimeKeeper.Instance.PhaseChanged += OnPhaseChanged;
        OnPhaseChanged();
    }

    void OnDestroy()
    {
        TimeKeeper.Instance.PhaseChanged -= OnPhaseChanged;
    }

    void OnPhaseChanged()
    {
        foreach (var r in GetComponentsInChildren<Renderer>())
        {
            r.material.SetFloat("_InvertFactor", TimeKeeper.Instance.Phase == GamePhase.Moving ? 1 : 0);
        }

        Camera.mainCamera.backgroundColor = TimeKeeper.Instance.Phase == GamePhase.Moving ?
            new Color(162 / 255f, 33 / 255f, 244 / 255f) : new Color(255 / 255f, 225 / 255f, 57 / 255f);

        RenderSettings.fogColor = TimeKeeper.Instance.Phase == GamePhase.Moving ?
            new Color(162 / 255f, 33 / 255f, 244 / 255f) : new Color(147 / 255f, 93 / 255f, 58 / 255f);
    }
}
