using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridFactory : MonoBehaviour
{
    [SerializeField] HexagonHelper hexagonHelperInstance;

    [SerializeField] int width = 5;
    [SerializeField] int depth = 5;

    [SerializeField] float elementOffset;
 
    void Start()
    {
        GenerateGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateGrid()
    {
        Vector3 origin = transform.position;
        int x = 0, z = 0;

        for(x = 0; x < width; x++)
        {
            GameObject hexagon  = hexagonHelperInstance.SpawnHexagon();
            
            Debug.LogFormat("w: {0}, d: {1}", (float)hexagonHelperInstance.hexagonWidth, (float)hexagonHelperInstance.hexagonDepth);
            Debug.LogFormat("x: {0}, z: {1}", x, z);
            hexagon.transform.position = new Vector3(origin.x + (float)hexagonHelperInstance.hexagonWidth * x, 0, origin.z + (float)hexagonHelperInstance.hexagonDepth * x);
            hexagon.transform.SetParent(transform);
            for (z = 0; z < depth; z++)
            {
                GameObject hexagon2  = hexagonHelperInstance.SpawnHexagon();
                hexagon2.transform.position = new Vector3(origin.x + (float)hexagonHelperInstance.hexagonWidth * x, 0, origin.z + (float)hexagonHelperInstance.hexagonDepth * x);
                hexagon2.transform.SetParent(transform);
            }
            
        }
    }
}
