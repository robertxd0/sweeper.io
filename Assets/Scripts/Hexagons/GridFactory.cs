using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//this provides... well the hexagons for the grid 🐱‍👤.
public class GridFactory : MonoBehaviour
{
      [SerializeField] private HexagonHelper hexagonHelperInstance;
      
      [SerializeField] private GameObject bombShopPrefab;
      [SerializeField] private GameObject upgradeShopPrefab;
      [SerializeField] private GameObject player;
 
      [SerializeField] private float playeZoneRadius = 5f;

      [SerializeField] internal int width = 5;
      [SerializeField] internal int depth = 5;

      internal int playerZoneCount = 0;


      [Range(1f, 2f)]
      [SerializeField] float spread = 1f;

      private static GridFactory instance;

      private readonly List<HexagonTile> spawnedHexagonTiles = new List<HexagonTile>();

      private List<Vector3> gridPoints;

      private BombShop bombShop;

      private UpgradeShop upgradeShop;


      public static GridFactory GetInstance()
      {
            return instance;
      }

      public List<HexagonTile> GetSpawnedPlates()
      {
            return this.spawnedHexagonTiles;
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

            GenerateGrid();
      }

      void GenerateGrid()
      {
            gridPoints = ComputeGridPoints().ToList();

            if (spawnedHexagonTiles.Count < gridPoints.Count)
            {
                  SpawnHexagons();
            }

            List<HexagonTile> tiles = new List<HexagonTile>();

            foreach (Vector3 pos in gridPoints)
            {
                  int index = gridPoints.IndexOf(pos);
                  spawnedHexagonTiles[index].transform.position = pos;

                  if(spawnedHexagonTiles[index].isTriggered)
                  {
                        tiles.Add(spawnedHexagonTiles[index]);
                  }
            }

            Physics.SyncTransforms();
            this.bombShop.Init();
            this.upgradeShop.Init();
            this.SetPlayerZone(player.transform);
      }

      void SpawnHexagons()
      {
            for (int i = 0; i < gridPoints.Count; i++)
            {
                  GameObject hexagon = hexagonHelperInstance.SpawnHexagon();
                  hexagon.name = "HexagonPlate no. " + i;
                  hexagon.transform.SetParent(transform);
                  HexagonTile tile = hexagon.GetComponent<HexagonTile>();
                  spawnedHexagonTiles.Add(tile);

                  if(i % 8 == 1 && this.bombShop == null) //ok, but not ok. but ok for now.
                  {
                       this.bombShop = PlaceBombFactory(tile);
                       continue;
                  }

                  if(i % 6 == 1 && this.upgradeShop == null)
                  {
                        this.upgradeShop = PlaceUpgradeShop(tile);
                        continue; 
                  }
            }
      }

      BombShop PlaceBombFactory(HexagonTile tile)
      {
            Vector3 bombShopPos = new Vector3(tile.transform.position.x, tile.transform.position.y + 1.5f, tile.transform.position.z);
            GameObject bombShopGO = GameObject.Instantiate(bombShopPrefab, bombShopPos, bombShopPrefab.transform.rotation);
            bombShopGO.transform.parent = tile.transform;

            return bombShopGO.GetComponent<BombShop>();
      }

      UpgradeShop PlaceUpgradeShop(HexagonTile tile)
      {
            Vector3 upgradeShopPos = new Vector3(tile.transform.position.x, tile.transform.position.y + 1.5f, tile.transform.position.z);
            GameObject upgradeShopGO = GameObject.Instantiate(upgradeShopPrefab, upgradeShopPos, upgradeShopPrefab.transform.rotation);
            upgradeShopGO.transform.parent = tile.transform;

            return upgradeShopGO.GetComponent<UpgradeShop>();
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

      void SetPlayerZone(Transform player)
      {
            Collider[] adjacentHexagons = Physics.OverlapSphere(player.position, playeZoneRadius);
            
            foreach(Collider col in adjacentHexagons)
            {
                  HexagonTile tileObj = col.GetComponent<HexagonTile>();
                  if(tileObj != null)
                  {
                        tileObj.InitPlateAsPlayerZone();
                        this.playerZoneCount++;
                  }     
            }
      }
}
