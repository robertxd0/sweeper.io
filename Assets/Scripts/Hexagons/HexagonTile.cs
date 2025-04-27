using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HexagonTile : MonoBehaviour
{
    public static TileTriggeredEvent TileTriggeredEvent = new TileTriggeredEvent();

    [SerializeField] Material borderPlateLook;
    [SerializeField] Material bombShopPlateLook;
    [SerializeField] Material upgradeShopPlateLook;

    [InspectorButton("TriggerTest")]
    public bool Trigger;

    internal bool isTriggered = false;
    internal bool borderPlate = false;

    private Dictionary<ShopFeedbackTypes, GameObject> shopFXFeedback;

    public bool hasBomb
    {
        get
        {
            return bomb != null;
        }
    }
    private Bomb bomb;
    private Resource resource;
    private Material standardPlateLook;

    private Collider[] adjacentPlatesCol;
    private HexagonTile[] adjacentPlatesObj;
    private bool isShopTile = false;
    private bool isUpgradeShopTile = false;

    private GameManager manager;

    private BombShop bombShopInstance;
    private UpgradeShop upgradeShopInstance;

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
                return;
            }
        }
        if (isShopTile)
        {
            if (other.tag == Constants.TAG_PLAYER)
            {
                this.bombShopInstance.StartPurchaseProcess(this);
            }
        }
    }

    private void OnTriggerStay(Collider other) {
        if(borderPlate && other.tag == Constants.TAG_PLAYER)
        {
            manager.HasBombs(); //(not ok) used for ui animation
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isShopTile)
        {
            if (other.tag == Constants.TAG_PLAYER)
            {
                this.bombShopInstance.InterruptPurchaseProcess();
                //sometimes be like that.

                foreach (GameObject feedbackObj in shopFXFeedback.Values)
                {
                    feedbackObj.SetActive(false);
                }
            }
        }
    }


    public void MarkAsShopTile(BombShop shopInstance)
    {

        GridFactory.GetInstance().playerZoneCount++; //dirty but works for now.
        this.InitPlateAsPlayerZone();
        this.bombShopInstance = shopInstance;

        //get feedback child GO
        if (shopFXFeedback == null)
        {
            shopFXFeedback = new Dictionary<ShopFeedbackTypes, GameObject>();
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            if (child.tag == ShopFeedbackTypes.SUCCESS.ToString().ToLower())
            {
                shopFXFeedback.Add(ShopFeedbackTypes.SUCCESS, child.gameObject);
            }
            else if (child.tag == ShopFeedbackTypes.FAILED.ToString().ToLower())
            {
                shopFXFeedback.Add(ShopFeedbackTypes.FAILED, child.gameObject);
            }
        }

        this.gameObject.layer = LayerMask.NameToLayer(Constants.LAYER_TRIGGERED_PLATES);
        this.GetComponent<MeshRenderer>().material = bombShopPlateLook;
        this.resource.gameObject.SetActive(false);
        this.isShopTile = true;
    }

    public bool IsShopPlate()
    {
        return isShopTile;
    }

    public void MarkAsUpgradeShopTile(UpgradeShop upgradeShopInstance)
    {
        // GridFactory.GetInstance().playerZoneCount++; //dirty but works for now.
        // this.InitPlateAsPlayerZone();
        this.upgradeShopInstance = upgradeShopInstance;
        this.GetComponent<MeshRenderer>().material = upgradeShopPlateLook;
        this.resource.gameObject.SetActive(false);
        this.gameObject.layer = LayerMask.NameToLayer(Constants.LAYER_TRIGGERED_PLATES);

        this.isUpgradeShopTile = true;
    }

    public bool IsUpgradeShopTile()
    {
        return isUpgradeShopTile;
    }

    public void EnableMarker()
    {
        this.resource.EnableMarker();
    }

    public void SetResource(Resource resourcePrefab)
    {
        if(!(this.isUpgradeShopTile && this.isShopTile))
        {
            resourcePrefab.gameObject.transform.SetParent(transform);
            resourcePrefab.gameObject.transform.localPosition = new Vector3(0f, 0f, 1.5f);

            this.resource = resourcePrefab;
        }
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
        if(!(this.isShopTile || this.isUpgradeShopTile))
        {
            this.borderPlate = true;
            this.bomb = null;
            this.gameObject.layer = LayerMask.NameToLayer(Constants.LAYER_TRIGGERED_PLATES); //it still is a triggered plate
            this.GetComponent<MeshRenderer>().material = borderPlateLook;

            if (this.resource != null && this.resource.gameObject.activeInHierarchy)
            {
                resource.Explode();
                UnsetResource();
            }
        }
    }

    public void UnsetBorderPlate()
    {
        this.borderPlate = false;
        this.gameObject.layer = LayerMask.NameToLayer(Constants.LAYER_TRIGGERED_PLATES);
        this.GetComponent<MeshRenderer>().material = standardPlateLook;

        if (this.resource != null && this.resource.gameObject.activeInHierarchy)
        {
            resource.Explode();
            UnsetResource();
        }
    }

    public void SetState(bool triggerReady)
    {
        MeshRenderer renderer = this.GetComponent<MeshRenderer>();
        if (triggerReady) renderer.material = this.borderPlateLook;
        else renderer.material = this.standardPlateLook;
    }

    private void TriggerPlate()
    {
        this.borderPlate = false;
        this.GetComponent<MeshRenderer>().material = this.standardPlateLook;

        int layer = 1 << LayerMask.NameToLayer(Constants.LAYER_UNTRIGGERED_PLATES);
        float radius = Bomb.bombStrength;

        this.adjacentPlatesCol = Physics.OverlapSphere(transform.position, radius, layer);

        //failsafe
        int tryCount = 0;
        while(adjacentPlatesCol.Length < 1 && tryCount < 15)
        {
            radius += 0.5f;
            tryCount++;
            this.adjacentPlatesCol = Physics.OverlapSphere(transform.position, radius, layer);
        }

        this.adjacentPlatesObj = new HexagonTile[adjacentPlatesCol.Length];

        int index = 0;
        foreach (Collider col in adjacentPlatesCol)
        {
            adjacentPlatesObj[index] = col.GetComponent<HexagonTile>();
            adjacentPlatesObj[index].isTriggered = true;
            adjacentPlatesObj[index].EnableMarker();
            adjacentPlatesCol[index].gameObject.layer = LayerMask.NameToLayer(Constants.LAYER_TRIGGERED_PLATES);
            index++;
        }

        HexagonTile.TileTriggeredEvent.Invoke(this);
    }

    public void TriggerPostExplosion()
    {

        if (adjacentPlatesCol.Length > 0 && adjacentPlatesObj.Length == adjacentPlatesCol.Length)
        {
            HexagonZoneManager.instance.AddHexagonsForPlayer(adjacentPlatesObj, 0);
        }
    }

    public void ExecuteShopFeedback(ShopFeedbackTypes feedbackType)
    {
        this.shopFXFeedback.GetValueOrDefault(feedbackType).SetActive(!this.shopFXFeedback.GetValueOrDefault(feedbackType).activeInHierarchy);
    }

    void Start()
    {
        this.standardPlateLook = this.GetComponent<MeshRenderer>().material;
        this.isTriggered = false;
        this.manager = GameManager.instance;
    }

    public void InitPlateAsPlayerZone()
    {
        this.isTriggered = true;
        this.resource.gameObject.SetActive(false);
        this.gameObject.layer = LayerMask.NameToLayer(Constants.LAYER_TRIGGERED_PLATES);
        HexagonTile[] plate = new HexagonTile[1] {this};
        HexagonZoneManager.instance.AddHexagonsForPlayer(plate, 0);
    }
}

[System.Serializable]
public class TileTriggeredEvent : UnityEvent<HexagonTile>
{
}
