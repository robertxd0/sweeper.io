using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//this provides... well the hexagons for the grid 🐱‍👤.
public class GridFactory : MonoBehaviour
{
    public static Vector3 gridBoundsMin
    {
        get
        {
            Vector3 origin = instance.transform.position;
            
            origin.x -= (float)instance.hexagonHelperInstance.hexagonWidth/2f;
            origin.z -= (float)instance.hexagonHelperInstance.hexagonDepth;

            return origin;
        }
    }

    public static Vector3 gridBoundsMax
    {
        get
        {
            Vector3 origin = instance.transform.position;
            
            origin.x += (float)(instance.hexagonHelperInstance.hexagonWidth * instance.spread * instance.width);
            origin.z += (float)((instance.hexagonHelperInstance.hexagonDepth * 0.7f) * instance.depth);

            return origin;
        }
    }

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

    [SerializeField] Transform bombShopPlacementPoint;
    [SerializeField] Transform upgradeShopPlacementPoint;

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

    void Awake()
    {
        hexagonHelperInstance.InitHelper();
    }

    void Start()
    {
        GenerateGrid();
        GenerateBounds();
    }

    void GenerateBounds()
    {
        GameObject boundsLeft = new GameObject("BoundsLeft", typeof(BoxCollider));
        GameObject boundsRight = new GameObject("BoundsRight", typeof(BoxCollider));
        GameObject boundsUp = new GameObject("BoundsUp", typeof(BoxCollider));
        GameObject boundsDown = new GameObject("BoundsDown", typeof(BoxCollider));

        boundsLeft.transform.SetParent(transform);
        boundsRight.transform.SetParent(transform);
        boundsUp.transform.SetParent(transform);
        boundsDown.transform.SetParent(transform);

        boundsLeft.transform.position = new Vector3(gridBoundsMin.x, 0f, Mathf.Abs(gridBoundsMin.z - gridBoundsMax.z) * 0.5f);
        BoxCollider colliderLeft = boundsLeft.GetComponent<BoxCollider>();
        colliderLeft.size = new Vector3(2f, 10f, Mathf.Abs(gridBoundsMax.z  - gridBoundsMin.z) + 2f);

        boundsRight.transform.position = new Vector3(gridBoundsMax.x, 0f, Mathf.Abs(gridBoundsMin.z - gridBoundsMax.z) * 0.5f);
        BoxCollider colliderRight = boundsRight.GetComponent<BoxCollider>();
        colliderRight.size = new Vector3(2f, 10f, Mathf.Abs(gridBoundsMax.z  - gridBoundsMin.z) + 2f);
    
        boundsUp.transform.position = new Vector3(Mathf.Abs(gridBoundsMin.x - gridBoundsMax.x) * 0.5f, 0f, gridBoundsMax.z);
        BoxCollider colliderUp = boundsUp.GetComponent<BoxCollider>();
        colliderUp.size = new Vector3(Mathf.Abs(gridBoundsMax.x - gridBoundsMin.x) + 2f, 10f, 2f);
        
        boundsDown.transform.position = new Vector3(Mathf.Abs(gridBoundsMin.x - gridBoundsMax.x) * 0.5f, 0f, gridBoundsMin.z);
        BoxCollider colliderDown = boundsDown.GetComponent<BoxCollider>();
        colliderDown.size = new Vector3(Mathf.Abs(gridBoundsMax.x - gridBoundsMin.x) + 2f, 10f, 2f);

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

            if (spawnedHexagonTiles[index].isTriggered)
            {
                tiles.Add(spawnedHexagonTiles[index]);
            }
        }

        Physics.SyncTransforms();
        this.PlaceInterestPoints();
        this.SetPlayerZone(player.transform);
    }

    void PlaceInterestPoints()
    {
        Collider[] tileCol = Physics.OverlapSphere(bombShopPlacementPoint.position, 0.1f);

        if (tileCol.Length > 0)
        {
            HexagonTile tile = tileCol[0].GetComponent<HexagonTile>();
            this.bombShop = this.PlaceBombFactory(tile);
            this.bombShop.Init();
        }

        tileCol = Physics.OverlapSphere(upgradeShopPlacementPoint.position, 0.1f);

        if (tileCol.Length > 0)
        {
            HexagonTile tile = tileCol[0].GetComponent<HexagonTile>();
            this.upgradeShop = PlaceUpgradeShop(tile);
            this.upgradeShop.Init();
        }
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

        foreach (Collider col in adjacentHexagons)
        {
            HexagonTile tileObj = col.GetComponent<HexagonTile>();
            if (tileObj != null)
            {
                tileObj.InitPlateAsPlayerZone();
                this.playerZoneCount++;
            }
        }
    }
}
