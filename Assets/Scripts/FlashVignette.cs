using UnityEngine;

class FlashVignette : MonoBehaviour
{
    public Texture2D VignetteTexture = null;

    public float FadeInPortion = 0.05f;
    public float FadeOutPortion = 0.25f;
    public float FadeOutMaxValue = 0.25f;

    void OnGUI()
    {
        float alpha = 0;
        TimeKeeper tk = TimeKeeper.Instance;

        switch (tk.Phase)
        {
            case GamePhase.Moving:
                if (tk.TimeRatio < FadeOutPortion)
                    alpha = (1 - Mathf.Clamp01(tk.TimeRatio / FadeOutPortion)) * FadeOutMaxValue;
                if (tk.TimeRatio > 1 - FadeInPortion)
                    alpha = Mathf.Clamp01((tk.TimeRatio - (1 - FadeInPortion)) / FadeInPortion);
                break;
        }

        if (alpha > 0)
        {
            GUI.color = new Color(1, 1, 1, alpha);
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), VignetteTexture);
        }
    }
}
