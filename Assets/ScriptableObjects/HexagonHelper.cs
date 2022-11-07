using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create hexagon helper instance", fileName = "HexagonHelperInstance")]
public class HexagonHelper : ScriptableObject
{
    public GameObject hexagonPrefab = null;

    public GameObject[] resourcePrefabs;

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
    
        return hexagonInstance;
    }
}