using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ShopFeedbackTypes
{
    SUCCESS,
    FAILED
}

//this is the in-game factory for bombs (ikr, shit naming..)👀
public class BombShop : MonoBehaviour
{
    [InspectorButton("Init")]
    public bool Initialize;


    private GameManager manager;
    private int bombPrice = 25;

    private Coroutine purchasingCoroutine;

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
                tileObj.MarkAsShopTile(this);
            }
        }
    }


    public void StartPurchaseProcess(HexagonTile originTile)
    {
        StartCoroutine(BombPurchaseCoroutine(0.075f, originTile));
    }

    public void InterruptPurchaseProcess()
    {
        StopAllCoroutines();
    }

    IEnumerator BombPurchaseCoroutine(float t, HexagonTile origin)
    {
        var end = Time.time + t;
        bool success = manager.IncrementBombs(bombPrice);
        origin.ExecuteShopFeedback(success ? ShopFeedbackTypes.SUCCESS : ShopFeedbackTypes.FAILED);
        
        while (Time.time < end)
        {
            yield return null;
        }

        StopAllCoroutines();
        StartCoroutine(BombPurchaseCoroutine(t, origin));
    }
}

