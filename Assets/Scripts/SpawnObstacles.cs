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

    float Rand(int i, int j, int n)
    {
        return PoisedNoise.UintToFloat(PoisedNoise.Hash((uint)i, (uint)j, (uint)n));
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
        r *= 1.25f;
        int treesToSpawn = Mathf.FloorToInt(r) +
            (Rand(i, j, 0x7fffffff) > Mathf.Repeat(r, 1) ? 0 : 1);


        for(int t = 0; t < treesToSpawn; t++)
        {
            int index = Mathf.FloorToInt(
                PoisedNoise.UintToFloat(PoisedNoise.Hash((uint)i, (uint)j,
                        (uint)t))*obstaclePrefabs.Count);
            if(index >= obstaclePrefabs.Count) index = obstaclePrefabs.Count-1;
            Transform prefab = obstaclePrefabs[index];
            Transform obstacle = (Transform)Instantiate(prefab,
                new Vector3(i+Rand(i,j,t*2), 0, j+Rand(i,j,t*2+1)) * CellSize,
                Quaternion.identity);
            obstacle.parent = go.transform;
        }

        int bubblesToSpawn = Mathf.FloorToInt(r/4) +
            (PoisedNoise.UintToFloat(PoisedNoise.Hash((uint)i, (uint)j, 0x7ffffffe))
                > Mathf.Repeat(r/4, 1) ? 0 : 1);
        for (int t = 0; t < bubblesToSpawn; t++)
        {
            Transform bubble = (Transform)Instantiate(bubblePrefab,
                new Vector3(i+Rand(i,j,t*3), 0, j+Rand(i,j,t*3+1)) * CellSize +
                    Vector3.up * (1 + Rand(i,j,t*3+2) * 8),
                Quaternion.identity);
            bubble.parent = go.transform;
        }

        return go;
    }
}
