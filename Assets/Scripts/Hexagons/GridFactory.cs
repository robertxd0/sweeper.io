using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//this provides... well the hexagons for the grid 🐱‍👤.
public class GridFactory : MonoBehaviour
{
      [SerializeField] private HexagonHelper hexagonHelperInstance;
      [SerializeField] private HexagonZoneManager hexagonZoneManager;

      [SerializeField] internal int width = 5;
      [SerializeField] internal int depth = 5;


      [Range(1f, 2f)]
      [SerializeField] float spread = 1f;

      private static GridFactory instance;

      private readonly List<HexagonPlate> spawnedHexagons = new List<HexagonPlate>();

      private List<Vector3> gridPoints;


      public static GridFactory GetInstance()
      {
            return instance;
      }

      public List<HexagonPlate> GetSpawnedPlates()
      {
            return this.spawnedHexagons;
      }

      private void OnEnable()
      {
            instance = this;
      }
      private void OnDisable()
      {
            instance = null;
      }
      private void OnDestroy()
      {
            instance = null;
      }

      void Start()
      {
            hexagonHelperInstance.InitHelper();
            hexagonZoneManager = GetComponent<HexagonZoneManager>();

            GenerateGrid();
      }

      void GenerateGrid()
      {
            gridPoints = ComputeGridPoints().ToList();

            if (spawnedHexagons.Count < gridPoints.Count)
            {
                  SpawnHexagons();
            }

            List<HexagonPlate> plates = new List<HexagonPlate>();

            foreach (Vector3 pos in gridPoints)
            {
                  int index = gridPoints.IndexOf(pos);
                  spawnedHexagons[index].transform.position = pos;

                  if(spawnedHexagons[index].isTriggered)
                  {
                        plates.Add(spawnedHexagons[index]);
                  }
            }

      }

      void SpawnHexagons()
      {
            for (int i = 0; i < gridPoints.Count; i++)
            {
                  GameObject hexagon = hexagonHelperInstance.SpawnHexagon();
                  hexagon.name = "HexagonPlate no. " + i;
                  hexagon.transform.SetParent(transform);
                  HexagonPlate plate = hexagon.GetComponent<HexagonPlate>();
                  spawnedHexagons.Add(plate);
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
                            origin.x + (float)hexagonHelperInstance.hexagonWidth * (x * spread) + ((z % 2 == 0) ? ((float)hexagonHelperInstance.hexagonWidth / 2f) + (spread - 1f) : 0),
                            0,
                            origin.z + (float)hexagonHelperInstance.hexagonDepth * (z * spread) - (z * (float)hexagonHelperInstance.hexagonWidth / 1.5f + (spread - 1f)));

                        yield return pos;
                  }
            }
      }
}
