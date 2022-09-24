using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create hexagon helper instance", fileName = "HexagonHelperInstance")]
public class HexagonHelper : ScriptableObject
{
    public GameObject hexagonPrefab = null;

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

        return hexagonInstance;
    }
}