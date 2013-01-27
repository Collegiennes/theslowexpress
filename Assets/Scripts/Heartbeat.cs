using UnityEngine;

class Heartbeat : MonoBehaviour
{
    void Start()
    {
        TimeKeeper.Instance.PhaseChanged += PlayHB;
    }

    void PlayHB()
    {
        if (!audio.isPlaying && TimeKeeper.Instance.Phase == GamePhase.Moving)
            audio.Play();
        else if (audio.isPlaying && TimeKeeper.Instance.Phase != GamePhase.Moving)
            audio.Pause();
    }
}
