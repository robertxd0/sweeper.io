using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//🐱‍🐉
public class HexagonZoneManager : MonoBehaviour
{
    public static HexagonZoneManager instance;

    private Dictionary<int, List<HexagonTile>> zones;

    private GridFactory factory;
    private Coroutine hexagonZoneCoroutine; 
    private List<HexagonTile> recentlyAddedPlates;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        zones = new Dictionary<int, List<HexagonTile>>();
        factory = GetComponent<GridFactory>();
    }


    IEnumerator MarkHexagonZoneWithDelay(int playerId) 
    {
        yield return new WaitForSeconds(0.25f);
        this.UpdateZone(playerId);
    }

    public void AddHexagonsForPlayer(HexagonTile[] hexagonPlates, int playerId)
    {
        if (!zones.ContainsKey(playerId))
        {
            List<HexagonTile> hexagons = new List<HexagonTile>();
            hexagons.AddRange(hexagonPlates);
            zones.Add(playerId, hexagons);     
        }
        else
            zones[playerId].AddRange(hexagonPlates);
        

        if (hexagonZoneCoroutine != null)
        {
            StopCoroutine(hexagonZoneCoroutine);
        }

        hexagonZoneCoroutine = StartCoroutine(MarkHexagonZoneWithDelay(playerId));
    }

    public void UpdateZone(int playerId)
    {
        if (!zones.ContainsKey(playerId))
        {
          return;
        }

        foreach (HexagonTile tile in zones[playerId])
        {
            if(tile.IsShopPlate())
            {
                continue;
            }    

            if (CheckBorderHexagon(tile))
            {
                tile.SetBorderPlate();
            }
            else
            {
                tile.UnsetBorderPlate();
            }
        }
    }

    bool CheckBorderHexagon(HexagonTile tile)
    {
        int layerMask = 1 << LayerMask.NameToLayer(Constants.LAYER_UNTRIGGERED_PLATES);

        if(Physics.OverlapSphere(tile.transform.position, 1.5f, layerMask).Length > 0){
            return true;
        }

        return false;
    }

}


