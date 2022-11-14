using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeShop : MonoBehaviour
{
    private GameManager manager;


    public void Init()
    {
        manager = GameManager.instance;

        int layerMask = 1 << LayerMask.NameToLayer(Constants.LAYER_TRIGGERED_PLATES);
        Collider[] adjacentTilesColliders = Physics.OverlapSphere(transform.position, 2f);
        foreach (Collider col in adjacentTilesColliders)
        {
            HexagonTile tileObj = col.GetComponent<HexagonTile>();
            if (tileObj != null)
            {
                tileObj.MarkAsUpgradeShopTile(this);
            }
        }
    }
}
