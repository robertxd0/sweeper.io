using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create hexagon helper instance", fileName = "HexagonHelperInstance")]
public class HexagonHelper : ScriptableObject
{
    public static int hexagonCount = 0;

    public GameObject hexagonPrefab = null;

    public GameObject[] resourcePrefabs;

    public GameObject[] coinResourcePrefabs;

    [Range(0f, 1f)]
    [SerializeField] private float GoldResourceQuota = 0.2f;    

    internal float? hexagonWidth;

    internal float? hexagonDepth;

    public void InitHelper()
    {
        if (hexagonWidth == null || hexagonDepth == null)
        {
            var tempInstance = Instantiate(hexagonPrefab);
            MeshRenderer hexagonMeshRenderer = tempInstance.GetComponent<MeshRenderer>();

            hexagonWidth = hexagonMeshRenderer.bounds.size.x;
            hexagonDepth = hexagonMeshRenderer.bounds.size.z;

            DestroyImmediate(tempInstance);
        }
    }

    public GameObject SpawnHexagon()
    {
        var hexagonInstance = Instantiate(hexagonPrefab);
        var resourceInstance = Instantiate(resourcePrefabs[Random.Range(0, resourcePrefabs.Length-1)]);
        
        HexagonTile plateObj = hexagonInstance.GetComponent<HexagonTile>();
        Resource resourceObj = resourceInstance.GetComponent<Resource>();

        plateObj.SetResource(resourceObj);

        HexagonHelper.hexagonCount++;
    
        return hexagonInstance;
    }

    Resource GetNewResource()
    {
        
    }
}