using UnityEngine;
using System.Collections;

public class SpawnObstacles : MonoBehaviour
{
    public Transform obstaclePrefab;
    public Transform bubblePrefab;

	void Start ()
    {
	    SpawnRegion();
	}
	
    void SpawnRegion()
    {
        for(int i = 0; i < 1000; i++)
        {
            Instantiate(obstaclePrefab,
                new Vector3(Random.value-0.5f, 0, Random.value) * 1000,
                Quaternion.identity);
        }

        for (int i = 0; i < 200; i++)
            Instantiate(bubblePrefab,
                new Vector3((Random.value - 0.5f) * 1000, Random.value * 8 + 1, Random.value * 1000),
                Quaternion.identity);
    }
}
