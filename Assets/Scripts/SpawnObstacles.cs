using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SpawnObstacles : MonoBehaviour
{
    public List<Transform> obstaclePrefabs;
    public Transform bubblePrefab;

    const float CellSize = 10;
    const int CellRange = 10;

    Dictionary<Vector2, GameObject> cells =
        new Dictionary<Vector2, GameObject>();

	void Update()
    {
        Vector3 playerPos = Camera.main.transform.position;
        int cx = Mathf.FloorToInt(playerPos.x/CellSize);
        int cz = Mathf.FloorToInt(playerPos.z/CellSize);

        foreach(Vector2 cellKey in
            cells.Keys.Where(k => (Mathf.Abs(k.x - cx) > CellRange ||
                                   Mathf.Abs(k.y - cz) > CellRange)).ToArray())
        {
            Destroy(cells[cellKey]);
            cells.Remove(cellKey);
        }

        for(int i = cx-CellRange; i <= cx+CellRange; i++)
        for(int j = cz-CellRange; j <= cz+CellRange; j++)
        {
            Vector2 cellKey = new Vector2(i, j);
            if(!cells.ContainsKey(cellKey))
            {
                cells[cellKey] = SpawnRegion(cellKey);
            }
        }
	}
	
    GameObject SpawnRegion(Vector2 cell)
    {
        GameObject go = new GameObject("Cell" + cell);
        int i = (int)cell.x;
        int j = (int)cell.y;

        float r = PoisedNoise.FourOctaveHash(i, j);
        r = (r * r)*0.75f+0.25f;
        //r *= 1 + Mathf.Abs(i/50.0f);
        r *= j/50.0f;
        int treesToSpawn = PoisedNoise.RandomRound(r * 1.25f);
        treesToSpawn = Mathf.FloorToInt(treesToSpawn) +
            (PoisedNoise.UintToFloat(PoisedNoise.Hash((uint)i, (uint)j, 0xffffffff))
                > Mathf.Repeat(treesToSpawn, 1) ? 0 : 1);


        for(int t = 0; t < treesToSpawn; t++)
        {
            int index = Mathf.FloorToInt(
                PoisedNoise.UintToFloat(PoisedNoise.Hash((uint)i, (uint)j,
                        (uint)t))*obstaclePrefabs.Count);
            if(index >= obstaclePrefabs.Count) index = obstaclePrefabs.Count-1;
            Transform prefab = obstaclePrefabs[index];
            Transform obstacle = (Transform)Instantiate(prefab,
                new Vector3(i+Random.value, 0, j+Random.value) * CellSize,
                Quaternion.identity);
            obstacle.parent = go.transform;
        }

        for (int t = 0; t < treesToSpawn / 4; t++)
        {
            Transform bubble = (Transform)Instantiate(bubblePrefab,
                new Vector3(i + Random.value, 0, j + Random.value) * CellSize +
                    Vector3.up * (1 + Random.value * 8),
                Quaternion.identity);
            bubble.parent = go.transform;
        }

        return go;
    }
}
