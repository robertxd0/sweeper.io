using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HexagonTile : MonoBehaviour
{
    public static PlateTriggeredEvent PlateTriggeredEvent = new PlateTriggeredEvent();

    [SerializeField] Material borderPlateLook;
    [SerializeField] Material shopPlateLook;
    
    [InspectorButton("TriggerTest")]
    public bool Trigger;

    internal bool isTriggered = false;
    internal bool borderPlate = false;

    
    public bool hasBomb {
        get{
            return bomb != null;
        }
    }
    private Bomb bomb;
    private Resource resource; 
    private Material standardPlateLook;

    private Collider[] adjacentPlatesCol;
    private HexagonTile[] adjacentPlatesObj;
    private bool isShopPlate = false;

    private GameManager manager;
 
    private void TriggerTest()
    {
       TriggerPlate();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (borderPlate)
        {
            if (other.tag == Constants.TAG_PLAYER && manager.HasBombs())
            {  
                TriggerPlate();
                manager.ConsumeBomb();
            }
        }
    }

    public void MarkAsShopTile()
    {
        this.GetComponent<MeshRenderer>().material = shopPlateLook;
        this.isShopPlate = true;
    }

    public bool IsShopPlate()
    {
        return isShopPlate;
    }

    public void EnableMarker()
    {
        this.resource.EnableMarker();
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

    public void SetBomb(Bomb bomb)
    {
        this.bomb = bomb;
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
        this.borderPlate = false;
        this.GetComponent<MeshRenderer>().material = this.standardPlateLook;

        int layer = 1 << LayerMask.NameToLayer(Constants.LAYER_UNTRIGGERED_PLATES);
        this.adjacentPlatesCol = Physics.OverlapSphere(transform.position, Bomb.bombStrength, layer);

        this.adjacentPlatesObj = new HexagonTile[adjacentPlatesCol.Length];

        int index = 0;
        foreach(Collider col in adjacentPlatesCol)
        {
            adjacentPlatesObj[index] = col.GetComponent<HexagonTile>();
            adjacentPlatesObj[index].isTriggered = true;
            adjacentPlatesObj[index].EnableMarker();
            adjacentPlatesCol[index].gameObject.layer = LayerMask.NameToLayer(Constants.LAYER_TRIGGERED_PLATES);
            index++;
        }

        HexagonTile.PlateTriggeredEvent.Invoke(this);
    }

    public void TriggerPostExplosion()
    {

        if(adjacentPlatesCol.Length > 0 && adjacentPlatesObj.Length == adjacentPlatesCol.Length)
        {
            HexagonZoneManager.instance.AddHexagonsForPlayer(adjacentPlatesObj, 0);
        }
    }

    void Start()
    {
        this.standardPlateLook = this.GetComponent<MeshRenderer>().material;
        this.isTriggered = false;
        this.manager = GameManager.instance;
        
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
                HexagonTile[] plate = new HexagonTile[1] {this};
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
public class PlateTriggeredEvent : UnityEvent<HexagonTile>
{
}
