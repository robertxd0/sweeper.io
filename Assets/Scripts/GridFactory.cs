using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GridFactory : MonoBehaviour
{
      [SerializeField] HexagonHelper hexagonHelperInstance;
      [SerializeField] HexagonZoneManager hexagonZoneManager;

      [SerializeField] int width = 5;
      [SerializeField] int depth = 5;


      [Range(1f, 2f)]
      [SerializeField] float spread = 1f;

      private static GridFactory instance;

      internal readonly List<HexagonPlate> spawnedHexagons = new List<HexagonPlate>();

      private List<Vector3> gridPoints;

      private Coroutine hexagonZoneCoroutine; // move to hexagonzonemanager when done testing
      private List<HexagonPlate> recentlyAddedPlates; // move to hexagonzonemanager when done testing
      public static GridFactory GetInstance()
      {
            return instance;
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

      public void PlateTriggered(HexagonPlate plate) //this should be managed by the hexaonzonemanager
      {
            int strength = plate.plateStrength; //strength = no. of plates to activate
            int[] directionArray = hexagonZoneManager.directionMappingArray;
            List<HexagonPlate> foundPlates = new List<HexagonPlate>();
            int index = spawnedHexagons.IndexOf(plate); //maybe it would be a good idea to split the spawned hexagons into two arrays(enabled, disabled)
            int tryCount = 20;
            while(foundPlates.Count() < strength)
            {
                  foreach(int factor in directionArray)
                  {
                        int realIndex = index - factor;
                        if(realIndex < 0 || realIndex > spawnedHexagons.Count) continue;

                        var hexagon = spawnedHexagons[realIndex];

                        if(!hexagon.isTriggered)
                        {
                              hexagon.gameObject.SetActive(true);
                              hexagon.isTriggered = true;
                              foundPlates.Add(hexagon);
                              AddPlate(hexagon);
                        }
                  }
                  tryCount++;
                  if(tryCount > 20 ) break;
            }

      }

      public void AddPlate(HexagonPlate plate)  // move to hexagonzonemanager when done testing  
      {
            if(hexagonZoneCoroutine != null)
            {
                  StopCoroutine(hexagonZoneCoroutine);
            }
            if(recentlyAddedPlates == null)
            {
                  recentlyAddedPlates = new List<HexagonPlate>();
            }
            if(!recentlyAddedPlates.Contains(plate))
                  recentlyAddedPlates.Add(plate);
            
            hexagonZoneCoroutine = StartCoroutine(MarkHexagonZoneWithDelay(recentlyAddedPlates.ToArray()));
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

      IEnumerator MarkHexagonZoneWithDelay(HexagonPlate[] plates) ///move this to the hexagon zone manager
      {
            yield return new WaitForSeconds(0.5f);
            hexagonZoneManager.UpdateZone(plates, 0);
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
