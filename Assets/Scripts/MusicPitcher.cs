using System.Collections;
using UnityEngine;

class MusicPitcher : MonoBehaviour
{
    void Start()
    {
        PlayerLevelling.Instance.Collided += () => StartCoroutine(AudioShake());
    }

    void Update()
    {
        audio.pitch = CoolSmooth.ExpoLinear(audio.pitch, TimeKeeper.Instance.TimeFactor, 0.9f, 0.25f,
                                            Time.deltaTime);
    }

    IEnumerator AudioShake()
    {
        float value = 0.75f;
        while (value >= Mathf.Epsilon)
        {
            value = CoolSmooth.ExpoLinear(value, 0, 0.99f, 2.5f, Time.deltaTime);
            audio.pitch += (Random.value - 0.5f) * value;
            yield return new WaitForEndOfFrame();
        }
    }
}
