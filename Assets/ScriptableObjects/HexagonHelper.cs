using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Create hexagon helper instance", fileName = "HexagonHelperInstance")]
public class HexagonHelper : ScriptableObject
{
    private int totalHexagonCount = 0;

    public GameObject hexagonPrefab = null;

    public GameObject[] resourcePrefabs;

    public GameObject[] goldResourcePrefabs;

    [Range(0f, 1f)]
    [SerializeField] private float goldResourceQuota = 0.2f;    

    internal float? hexagonWidth;

    internal float? hexagonDepth;

    private int[] goldResourceIndices;

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

        this.totalHexagonCount = 0;
        this.goldResourceIndices = new int[0];
    }

    public GameObject SpawnHexagon()
    {
        this.totalHexagonCount++;
        
        var hexagonInstance = Instantiate(hexagonPrefab);
        var resource = GetNewResource();

        HexagonTile plateObj = hexagonInstance.GetComponent<HexagonTile>();

        plateObj.SetResource(resource);
    
        return hexagonInstance;
    }

    Resource GetNewResource()
    {
        if(goldResourceIndices.Length == 0)
            goldResourceIndices = GenerateGoldResourceIndices();
        
        GameObject resourceInstance = null;

        if(goldResourceIndices.Contains(this.totalHexagonCount))
        {
            resourceInstance = Instantiate(goldResourcePrefabs[Random.Range(0, goldResourcePrefabs.Length-1)]);
        }else
        {
            resourceInstance = Instantiate(resourcePrefabs[Random.Range(0, resourcePrefabs.Length-1)]);
        }

        Resource resourceObj = resourceInstance.GetComponent<Resource>();

        return resourceObj;
    }

    int[] GenerateGoldResourceIndices()
    {
        int totalHexagonCount = GridFactory.GetInstance().depth * GridFactory.GetInstance().width;
        int totalGoldResourceCount = (int)((float)totalHexagonCount * (goldResourceQuota + 0.15f)); // 0.15f, to account for the hexagons that don't show any resources because of the spawn point.

        int[] indices = new int[totalGoldResourceCount];

        for(int i = 0; i < indices.Length; i++)
        {
            int index = Random.Range(0, totalHexagonCount);
            int tryCount = 0;
            
            //very dirty. better solution needed, if this passes the prototyping phase.
            while(indices.Contains(index) && tryCount < 10)
            {
                index = Random.Range(0, totalHexagonCount);
                tryCount++;
            }

            indices[i] = index;
        }

        return indices;

    }

    
}