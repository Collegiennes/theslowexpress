using UnityEngine;
using System.Collections;

public class TailSpawner : MonoBehaviour
{
    public Follow tailPrefab;
    public int length = 5;

	void Start ()
    {
        Transform target = transform;
	    for(int i = 0; i < length; i++)
        {
            Follow tailElement = (Follow)Instantiate(
                tailPrefab, target.transform.position + new Vector3(0, 0, -0.75f), transform.rotation);
            tailElement.target = target;
            target = tailElement.transform;
        }
	}
}
