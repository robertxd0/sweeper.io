using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this is the in-game factory for bombs (ikr, shit naming..)👀
public class BombShop : MonoBehaviour
{
     [InspectorButton("Init")]
    public bool Initialize;

    public void Init()
    {
        int layerMask = 1 << LayerMask.NameToLayer(Constants.LAYER_TRIGGERED_PLATES);
        Collider[] adjacentTilesColliders = Physics.OverlapSphere(transform.position, 2f);
        foreach(Collider col in adjacentTilesColliders)
        {
            HexagonTile tileObj = col.GetComponent<HexagonTile>();
            if(tileObj != null)
            {
                tileObj.MarkAsShopTile();
            }
        }
    }
}
