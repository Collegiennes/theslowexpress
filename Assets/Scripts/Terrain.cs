using UnityEngine;
using System.Collections;

public class Terrain : MonoBehaviour
{
    public Transform obstaclePrefab;

	void Start ()
    {
	    SpawnRegion();
	}
	
    void SpawnRegion()
    {
        for(int i = 0; i < 100; i++)
        {
            Instantiate(obstaclePrefab,
                new Vector3(Random.value-0.5f, 0, Random.value-0.5f) * 200,
                Quaternion.identity);
        }
    }
}
