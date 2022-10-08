using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonPlate : MonoBehaviour
{
    [SerializeField] Material triggerablePlateLook;

    private Material standardPlateLook;

    internal bool isTriggered;

    internal int plateStrength;

     [InspectorButton("TriggerTest")]
    public bool Trigger;

    private void TriggerTest()
    {
        this.plateStrength = Random.Range(1, 10);
        this.GetComponent<MeshRenderer>().material = this.standardPlateLook;
        HexagonZoneManager.instance.PlateTriggered(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isTriggered)
        {
            if (other.tag == Constants.TAG_PLAYER)
            {   //enscapsulate this into a function
                this.plateStrength = Random.Range(1, 5);
                this.GetComponent<MeshRenderer>().material = this.standardPlateLook;
                HexagonZoneManager.instance.PlateTriggered(this);
            }
        }
    }

    public void setTriggerable()
    {
        this.GetComponent<MeshRenderer>().material = triggerablePlateLook;
    }

    public void setState(bool triggerReady)
    {
        MeshRenderer renderer = this.GetComponent<MeshRenderer>();
        if(triggerReady) renderer.material = this.triggerablePlateLook;
        else renderer.material = this.standardPlateLook;
            
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
        bool plateState = false;
        Collider[] overlappingColliders = Physics.OverlapSphere(transform.position, 15f);
        foreach (Collider col in overlappingColliders)
        {
            if (col.tag == Constants.TAG_PLAYER)
            {
                plateState = true;
                this.isTriggered = true;
                HexagonZoneManager.instance.AddPlate(this);
            }
        }
        this.gameObject.SetActive(plateState);
    }


}
