using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{
    void Start()
    {
        int layerMask = 1 << LayerMask.NameToLayer(Constants.LAYER_TRIGGERED_PLATES);
        Collider[] adjacentTilesColliders = Physics.OverlapSphere(transform.position, 1.5f);
        foreach(Collider col in adjacentTilesColliders)
        {
            HexagonTile tileObj = col.GetComponent<HexagonTile>();
            if(tileObj != null)
            {
                tileObj.MarkAsShopTile();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
