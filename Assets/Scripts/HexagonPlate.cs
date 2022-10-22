using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HexagonPlate : MonoBehaviour
{
    public static PlateTriggeredEvent PlateTriggeredEvent = new PlateTriggeredEvent();

    [SerializeField] Material triggerablePlateLook;
    [SerializeField] Material untriggeredPlateLook; 

    private Material standardPlateLook;

    internal bool isTriggered;
    
    internal bool triggerable = false;


    [InspectorButton("TriggerTest")]
    public bool Trigger;

    private void TriggerTest()
    {
       TriggerPlate();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggerable && isTriggered)
        {
            if (other.tag == Constants.TAG_PLAYER)
            {  
                TriggerPlate();
            }
        }
    }

    public void SetTriggerable()
    {
        this.triggerable = true;
        this.gameObject.layer = LayerMask.NameToLayer(Constants.LAYER_TRIGGERED_PLATES);
        this.GetComponent<MeshRenderer>().material = triggerablePlateLook;
    }

    public void SetUnTriggerable()
    {
        this.triggerable = false;
        this.gameObject.layer = LayerMask.NameToLayer(Constants.LAYER_TRIGGERED_PLATES);
        this.GetComponent<MeshRenderer>().material =  standardPlateLook;
    }

    public void SetState(bool triggerReady)
    {
        MeshRenderer renderer = this.GetComponent<MeshRenderer>();
        if(triggerReady) renderer.material = this.triggerablePlateLook;
        else renderer.material = this.standardPlateLook;
    }

    private void TriggerPlate()
    {
        this.isTriggered = true;
        this.triggerable = false;
        this.GetComponent<MeshRenderer>().material = this.standardPlateLook;

        // float plateStrength = Random.Range(2f, 6f);
        float plateStrength = 3f;

        int layer = 1 << LayerMask.NameToLayer(Constants.LAYER_UNTRIGGERED_PLATES);
        Collider[] adjacentPlatesCol = Physics.OverlapSphere(transform.position, plateStrength, layer);

        HexagonPlate[] adjacentPlatesObj = new HexagonPlate[adjacentPlatesCol.Length];

        int index = 0;
        foreach(Collider col in adjacentPlatesCol)
        {
            adjacentPlatesObj[index] = col.GetComponent<HexagonPlate>();
            adjacentPlatesObj[index].isTriggered = true;
            adjacentPlatesCol[index].gameObject.layer = LayerMask.NameToLayer(Constants.LAYER_TRIGGERED_PLATES);
            index++;
        }
        
        if(adjacentPlatesCol.Length > 0 && adjacentPlatesObj.Length == adjacentPlatesCol.Length)
        {
            HexagonZoneManager.instance.AddHexagonsForPlayer(adjacentPlatesObj, 0);
        }

        HexagonPlate.PlateTriggeredEvent.Invoke(this);
    }

    void Start()
    {
        this.standardPlateLook = this.GetComponent<MeshRenderer>().material;
        this.isTriggered = false;
        InitPlateState();
    }

    void InitPlateState()
    {
        //check wether player is near this tile
        //bool plateState = false;
        Collider[] overlappingColliders = Physics.OverlapSphere(transform.position, 15f); // add layer mask
        foreach (Collider col in overlappingColliders)
        {
            if (col.tag == Constants.TAG_PLAYER)
            {
                //plateState = true;
                this.isTriggered = true;
                HexagonPlate[] plate = new HexagonPlate[1] {this};
                HexagonZoneManager.instance.AddHexagonsForPlayer(plate, 0);
            }
        }
        if(!this.isTriggered)
        {
            this.GetComponent<MeshRenderer>().material = this.untriggeredPlateLook;
            this.gameObject.layer = LayerMask.NameToLayer(Constants.LAYER_UNTRIGGERED_PLATES);
        }
        //this.gameObject.SetActive(plateState);
    }
}

[System.Serializable]
public class PlateTriggeredEvent : UnityEvent<HexagonPlate>
{
}
