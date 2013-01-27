using UnityEngine;

class Guru : MonoBehaviour
{
    public Material Dream;
    public Material Shoot;

    void Start()
    {
        TimeKeeper.Instance.PhaseChanged += SwitchMaterials;
        SwitchMaterials();
    }

    void SwitchMaterials()
    {
        if (TimeKeeper.Instance.Phase == GamePhase.Grabbing)
            GetComponentInChildren<Renderer>().material = Shoot;
        else
            GetComponentInChildren<Renderer>().material = Dream;
    }
}
