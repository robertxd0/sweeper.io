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

    public void PlateTriggered(HexagonPlate plate) //this should be managed by the hexaonzonemanager
    {
        //------------------------OLD LOGIC-----------------------------------------
        // int strength = plate.plateStrength; //strength = no. of plates to activate
        // var spawnedPlates = factory.GetSpawnedPlates();
        // List<HexagonPlate> foundPlates = new List<HexagonPlate>();
        // int index = spawnedPlates.IndexOf(plate); //maybe it would be a good idea to split the spawned hexagons into two arrays(enabled, disabled)
        // int tryCount = 20;
        // while (foundPlates.Count < strength)
        // {
        //     foreach (int factor in directionArray)
        //     {
        //         int realIndex = index - factor;
        //         if (realIndex < 0 || realIndex > spawnedPlates.Count - 1) continue;

        //         var hexagon = spawnedPlates[realIndex];

        //         if (!hexagon.isTriggered && !hexagon.isActiveAndEnabled)
        //         {
        //             hexagon.gameObject.SetActive(true);
        //             hexagon.isTriggered = true;
        //             foundPlates.Add(hexagon);
        //             AddPlate(hexagon);
        //         }
        //     }
        //     tryCount++;
        //     if (tryCount > 20) break;
        // }

        //------------------------OLD LOGIC END--------------------------------------
        var currentPlate = plate;
        int strength = plate.plateStrength;
        var spawnedPlates = factory.GetSpawnedPlates();
        int tryCount = 0;
        while (tryCount < strength)
        {

            int nextIndex;
            CheckBorderHexagon(currentPlate, out nextIndex);
            if (nextIndex != -1)
            {
                currentPlate = spawnedPlates[nextIndex];

                currentPlate.gameObject.SetActive(true);
                currentPlate.isTriggered = true;
                zones[0].Add(currentPlate);
                //AddPlate(currentPlate);
            }
            tryCount++;
            if (tryCount > 5) break;
        }
        ReloadZone(0); // use the id here
    }

    public void ReloadZone(int playerID)
    {
        UpdateZone(zones[0].ToArray(), 0);
    }

    public void AddPlate(HexagonPlate plate)  // move to hexagonzonemanager when done testing  
    {
        if (hexagonZoneCoroutine != null)
        {
            StopCoroutine(hexagonZoneCoroutine);
        }
        if (recentlyAddedPlates == null)
        {
            recentlyAddedPlates = new List<HexagonPlate>();
        }
        if (!recentlyAddedPlates.Contains(plate))
            recentlyAddedPlates.Add(plate);

        hexagonZoneCoroutine = StartCoroutine(MarkHexagonZoneWithDelay(recentlyAddedPlates.ToArray()));
    }

    IEnumerator MarkHexagonZoneWithDelay(HexagonPlate[] plates) ///move this to the hexagon zone manager
    {
        yield return new WaitForSeconds(0.5f);
        this.UpdateZone(plates, 0);
    }

    public void AddHexagonsForPlayer(HexagonPlate[] hexagonPlates, int playerId)
    {
        if (zones.ContainsKey(playerId))
        {
            zones[playerId].AddRange(hexagonPlates);
            return;
        }
        else
        {
            List<HexagonPlate> hexagons = new List<HexagonPlate>();
            hexagons.AddRange(hexagonPlates);
            zones.Add(playerId, hexagons);
        }

        this.UpdateZone(hexagonPlates, playerId);
    }

    public void UpdateZone(HexagonPlate[] hexagonPlates, int playerId)
    {
        if (!zones.ContainsKey(playerId))
        {
            List<HexagonPlate> plates = new List<HexagonPlate>();
            plates.AddRange(hexagonPlates);
            zones.Add(playerId, plates);
        }else
            zones[playerId].AddRange(hexagonPlates);

        foreach (HexagonPlate plate in hexagonPlates)
        {
            if (CheckBorderHexagon(plate))
            {
                plate.setTriggerable();
            }
        }
    }


    public bool CheckBorderHexagon(HexagonPlate plate)
    {
        return CheckBorderHexagon(plate, out _);
    }


    //Determines wether the hexagon lays at the border of the zone and marks it as such
    bool CheckBorderHexagon(HexagonPlate plate, out int nextPlateIndex)
    {
        var spawnedPlates = factory.GetSpawnedPlates();
        int index = spawnedPlates.IndexOf(plate);


        if (index % factory.depth != 0 && index % factory.width != 0)
        {
            int targetIndex = -1;
            var hexagon = spawnedPlates[index];

            //left (n-depth)
            targetIndex = index - factory.depth;

            if (targetIndex > 0 && targetIndex < spawnedPlates.Count)
            {
                var adjacentHexagon = spawnedPlates[targetIndex];

                if (!adjacentHexagon.isTriggered && !adjacentHexagon.gameObject.activeInHierarchy)
                {
                    nextPlateIndex = targetIndex;
                    return true;
                }
            }

            //right (n+depth)
            targetIndex = index + factory.depth;

            if (targetIndex > 0 && targetIndex < spawnedPlates.Count)
            {
                var adjacentHexagon = spawnedPlates[targetIndex];

                if (!adjacentHexagon.isTriggered && !adjacentHexagon.gameObject.activeInHierarchy)
                {
                    nextPlateIndex = targetIndex;
                    return true;
                }
            }

            //upper left (n-(depth-1))
            targetIndex = index - (factory.depth - 1);

            if (targetIndex > 0 && targetIndex < spawnedPlates.Count)
            {
                var adjacentHexagon = spawnedPlates[targetIndex];

                if (!adjacentHexagon.isTriggered && !adjacentHexagon.gameObject.activeInHierarchy)
                {
                    nextPlateIndex = targetIndex;
                    return true;
                }

            }

            //upper right (n+1)
            targetIndex = index + 1;

            if (targetIndex > 0 && targetIndex < spawnedPlates.Count)
            {
                var adjacentHexagon = spawnedPlates[targetIndex];

                if (!adjacentHexagon.isTriggered && !adjacentHexagon.gameObject.activeInHierarchy)
                {
                    nextPlateIndex = targetIndex;
                    return true;
                }

            }

            //lower left (n-(depth+1))
            targetIndex = index - (factory.depth + 1);

            if (targetIndex > 0 && targetIndex < spawnedPlates.Count)
            {
                var adjacentHexagon = spawnedPlates[targetIndex];

                if (!adjacentHexagon.isTriggered && !adjacentHexagon.gameObject.activeInHierarchy)
                {
                    nextPlateIndex = targetIndex;
                    return true;
                }

            }

            //lower right (n-1)
            targetIndex = index - 1;

            if (targetIndex > 0 && targetIndex < spawnedPlates.Count)
            {
                var adjacentHexagon = spawnedPlates[targetIndex];

                if (!adjacentHexagon.isTriggered && !adjacentHexagon.gameObject.activeInHierarchy)
                {
                    nextPlateIndex = targetIndex;
                    return true;
                }

            }

        }

        //no adjacent untriggered hexagon
        nextPlateIndex = -1;
        return false;

    }

}


