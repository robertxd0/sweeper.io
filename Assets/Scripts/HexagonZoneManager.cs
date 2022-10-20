using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonZoneManager : MonoBehaviour
{
    public static HexagonZoneManager instance;

    [SerializeField] public int[] directionMappingArray; //this should not be public 

    private Dictionary<int, List<HexagonPlate>> zones;

    private GridFactory factory;
    private Coroutine hexagonZoneCoroutine; // move to hexagonzonemanager when done testing
    private List<HexagonPlate> recentlyAddedPlates; // move to hexagonzonemanager when done testing


    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        zones = new Dictionary<int, List<HexagonPlate>>();
        factory = GetComponent<GridFactory>();
    }


    IEnumerator MarkHexagonZoneWithDelay(int playerId) ///move this to the hexagon zone manager
    {
        yield return new WaitForSeconds(0.25f);
        this.UpdateZone(playerId);
    }

    public void AddHexagonsForPlayer(HexagonPlate[] hexagonPlates, int playerId)
    {
        if (!zones.ContainsKey(playerId))
        {
            List<HexagonPlate> hexagons = new List<HexagonPlate>();
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

        foreach (HexagonPlate plate in zones[playerId])
        {
            if (CheckBorderHexagon(plate))
            {
                plate.SetTriggerable();
            }
            else
            {
                plate.SetUnTriggerable();
            }
        }
    }

    bool CheckBorderHexagon(HexagonPlate plate)
    {
        int layerMask = 1 << LayerMask.NameToLayer(Constants.LAYER_UNTRIGGERED_PLATES);

        if(Physics.OverlapSphere(plate.transform.position, 1.5f, layerMask).Length > 0){
            return true;
        }

        return false;
    }

}


