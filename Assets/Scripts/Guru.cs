using System.Collections;
using UnityEngine;

class Guru : MonoBehaviour
{
    public Material Dream = null;
    public Material Shoot = null;
    public Material Hurt = null;

    bool changed = false;

    void Start()
    {
        TimeKeeper.Instance.PhaseChanged += SwitchMaterials;
        SwitchMaterials();

        PlayerLevelling.Instance.Collided += () => StartCoroutine(ChangeHit());
    }

    IEnumerator ChangeHit()
    {
        changed = false;

        var was = GetComponentInChildren<Renderer>().material;

        GetComponentInChildren<Renderer>().material = Hurt;
        yield return new WaitForSeconds(1.25f * TimeKeeper.Instance.TimeFactor);

        if (!changed)
        {
            bool alt = false;
            for (int i = 0; i < 6; i++)
            {
                GetComponentInChildren<Renderer>().enabled = alt;
                yield return new WaitForSeconds(0.075f * TimeKeeper.Instance.TimeFactor);
                alt = !alt;
            }
            GetComponentInChildren<Renderer>().material = was;
            GetComponentInChildren<Renderer>().enabled = true;
        }
    }

    void SwitchMaterials()
    {
        if (TimeKeeper.Instance.Phase == GamePhase.Grabbing)
            GetComponentInChildren<Renderer>().material = Shoot;
        else
            GetComponentInChildren<Renderer>().material = Dream;

        changed = true;
    }
}
