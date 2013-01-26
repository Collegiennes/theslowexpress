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
                tailPrefab, transform.position, transform.rotation);
            tailElement.target = target;
            target = tailElement.transform;
        }
	}
}
