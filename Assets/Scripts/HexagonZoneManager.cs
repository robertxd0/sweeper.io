using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonZoneManager : MonoBehaviour
{
    [SerializeField] public int[] directionMappingArray; //this should not be public 

    private Dictionary<int, List<HexagonPlate>> zones;

    private GridFactory factory;

    public static HexagonZoneManager instance;


private void Awake() {
    instance = this;    
}
    void Start()
    {
        zones = new Dictionary<int, List<HexagonPlate>>();
        factory = GetComponent<GridFactory>();
    }

    public void AddHexagonsForPlayer(HexagonPlate[] hexagonPlates, int playerId)
    {
        if(zones.ContainsKey(playerId))
        {
            zones[playerId].AddRange(hexagonPlates);
            return;
        }else
        {    
            List<HexagonPlate> hexagons = new List<HexagonPlate>();
            hexagons.AddRange(hexagonPlates);        
            zones.Add(playerId, hexagons);
        }
        
        this.UpdateZone(hexagonPlates, playerId);
    }

    public void UpdateZone(HexagonPlate[] hexagonPlates, int playerId)
    {
        int index = -1;

        foreach (HexagonPlate plate in hexagonPlates)
        {
            index = factory.spawnedHexagons.IndexOf(plate);
            if(index == 0)
                continue;

            foreach(int directionValue in directionMappingArray)
            {
                int actualValue = index + directionValue;
                if (actualValue < 0 || actualValue > factory.spawnedHexagons.Count) continue;

                var nHexagon = factory.spawnedHexagons[actualValue];
                if(!nHexagon.isTriggered)
                {
                    plate.setTriggerable();
                }
            }

            
        }
    }

}


