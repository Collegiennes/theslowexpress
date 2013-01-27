using UnityEngine;

class Hand : MonoBehaviour
{
    public int Direction = 0;

    public Material IdleMaterial = null;
    public Material PointMaterial = null;
    public Material GrabMaterial = null;

    void Start()
    {
        TimeKeeper.Instance.PhaseChanged += () =>
        {
            var visible = TimeKeeper.Instance.Phase == GamePhase.Grabbing;

            enabled = visible;
            foreach (var r in GetComponentsInChildren<Renderer>())
                r.enabled = visible;
        };
    }

    void Update()
    {
        //float 

       // if (Direction == -1)

    }
}
