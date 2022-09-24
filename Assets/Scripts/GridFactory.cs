using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GridFactory : MonoBehaviour
{
    [SerializeField] HexagonHelper hexagonHelperInstance;

    [SerializeField] int width = 5;
    [SerializeField] int depth = 5;


    [Range(1f, 2f)]
    [SerializeField] float spread = 1f;

    private List<Vector3> gridPoints;
    private readonly List<GameObject> spawnedHexagons = new List<GameObject>();


    void Start()
    {
        hexagonHelperInstance.InitHelper();
        GenerateGrid();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void GenerateGrid()
    {
        gridPoints = ComputeGridPoints().ToList();

        if (spawnedHexagons.Count < gridPoints.Count)
        {
            SpawnHexagons();
        }

        foreach (Vector3 pos in gridPoints)
        {
            int index = gridPoints.IndexOf(pos);
            spawnedHexagons[index].transform.position = pos;
        }
    }

    void SpawnHexagons()
    {
        for (int i = 0; i < gridPoints.Count; i++)
        {
            GameObject hexagon = hexagonHelperInstance.SpawnHexagon();
            hexagon.name = "HexagonPlate no. " + i + 1;
            hexagon.transform.SetParent(transform);
            spawnedHexagons.Add(hexagon);
        }
    }


    IEnumerable<Vector3> ComputeGridPoints()
    {
        Vector3 origin = transform.position;
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                var pos = new Vector3(
                    origin.x + (float)hexagonHelperInstance.hexagonWidth * (x * spread) + ((z % 2 == 0) ? ((float)hexagonHelperInstance.hexagonWidth / 2) + (spread - 1) : 0),
                    0,
                    origin.z + (float)hexagonHelperInstance.hexagonDepth * (z * spread));

                yield return pos;
            }
        }
    }
}
