using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HexagonPlate : MonoBehaviour
{
    public static PlateTriggeredEvent PlateTriggeredEvent = new PlateTriggeredEvent();

    [SerializeField] Material borderPlateLook;
    
    [InspectorButton("TriggerTest")]
    public bool Trigger;

    internal bool isTriggered = false;
    internal bool borderPlate = false;
    internal bool hasBomb = false;
    private Resource resource; 
    private Material standardPlateLook;


    private void TriggerTest()
    {
       TriggerPlate();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (borderPlate)
        {
            if (other.tag == Constants.TAG_PLAYER)
            {  
                TriggerPlate();
            }
        }
    }

    public void SetResource(Resource resourcePrefab)
    {
        resourcePrefab.gameObject.transform.SetParent(transform);
        resourcePrefab.gameObject.transform.localPosition = new Vector3(0f, 0f, 1.5f);

        this.resource = resourcePrefab;
    }

    private void UnsetResource()
    {
        //maybe pooling?
        this.resource = null;
    }

    public void SetBorderPlate()
    {
        this.borderPlate = true;
        this.gameObject.layer = LayerMask.NameToLayer(Constants.LAYER_TRIGGERED_PLATES); //it still is a triggered plate
        this.GetComponent<MeshRenderer>().material = borderPlateLook;

        if(this.resource != null && this.resource.gameObject.activeInHierarchy)
        {
            resource.Explode();
            UnsetResource();
        }
    }

    public void UnsetBorderPlate()
    {
        this.borderPlate = false;
        this.gameObject.layer = LayerMask.NameToLayer(Constants.LAYER_TRIGGERED_PLATES);
        this.GetComponent<MeshRenderer>().material =  standardPlateLook;

        if(this.resource != null && this.resource.gameObject.activeInHierarchy)
        {
            resource.Explode();
            UnsetResource();
        }
    }

    public void SetState(bool triggerReady)
    {
        MeshRenderer renderer = this.GetComponent<MeshRenderer>();
        if(triggerReady) renderer.material = this.borderPlateLook;
        else renderer.material = this.standardPlateLook;
    }

    private void TriggerPlate()
    {
        HexagonPlate.PlateTriggeredEvent.Invoke(this);
    }

    public void TriggerPostExplosion()
    {
        this.borderPlate = false;
        this.GetComponent<MeshRenderer>().material = this.standardPlateLook;

        // float plateStrength = Random.Range(2f, 6f);
        float plateStrength = 2f;

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
        Collider[] overlappingColliders = Physics.OverlapSphere(transform.position, 10f); // add layer mask
        foreach (Collider col in overlappingColliders)
        {
            if (col.tag == Constants.TAG_PLAYER)
            {
                //plateState = true;
                this.isTriggered = true;
                this.resource.gameObject.SetActive(false);
                HexagonPlate[] plate = new HexagonPlate[1] {this};
                HexagonZoneManager.instance.AddHexagonsForPlayer(plate, 0);
            }
        }
        if(!this.isTriggered)
        {
            //this.GetComponent<MeshRenderer>().material = this.untriggeredPlateLook; OBSOLETE
            this.gameObject.layer = LayerMask.NameToLayer(Constants.LAYER_UNTRIGGERED_PLATES);
        }
        //this.gameObject.SetActive(plateState);
    }
}

[System.Serializable]
public class PlateTriggeredEvent : UnityEvent<HexagonPlate>
{
}
