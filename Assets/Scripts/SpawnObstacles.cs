using UnityEngine;
using System.Collections.Generic;

public class SpawnObstacles : MonoBehaviour
{
    public List<Transform> obstaclePrefabs;
    public Transform bubblePrefab;

	void Start ()
    {
	    SpawnRegion();
	}
	
    void SpawnRegion()
    {
        for(int i = -20; i <= 20; i++)
        for(int j = 0; j < 50; j++)
        {
            float r = PoisedNoise.FourOctaveHash(i+1000, j+100000);
            r = r * r;
            int treesToSpawn = Mathf.FloorToInt(r * 10);
            //treesToSpawn += (j-50)/10;

            for(int t = 0; t < treesToSpawn; t++)
            {
                Transform prefab = obstaclePrefabs[Mathf.FloorToInt(
                    PoisedNoise.UintToFloat(PoisedNoise.Hash((uint)i, (uint)j, (uint)t))*obstaclePrefabs.Count)];
                Instantiate(prefab,
                    new Vector3(i+Random.value, 0, j+Random.value) * 30,
                    Quaternion.identity);
            }
        }
        for (int i = 0; i < 200; i++)
        {
            Instantiate(bubblePrefab,
                new Vector3((Random.value - 0.5f) * 1000,
                            Random.value * 8 + 1,
                            Random.value * 1000),
                Quaternion.identity);
        }
    }
}
