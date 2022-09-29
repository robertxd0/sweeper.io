using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonPlate : MonoBehaviour
{
    [SerializeField] Material triggerablePlateLook;

    internal bool isTriggered;

    internal int plateStrength;

    private void OnTriggerEnter(Collider other)
    {
        if (isTriggered)
        {
            if (other.tag == Constants.TAG_PLAYER)
            {   //capsulate this into a function
                this.plateStrength = Random.Range(1, 5);
                //this.isTriggered = true;
                //this.gameObject.SetActive(true); 
                GridFactory.GetInstance().PlateTriggered(this); // the zonemanager should handle this, not grid factory
            }
        }
    }

    public void setTriggerable()
    {
        this.GetComponent<MeshRenderer>().material = triggerablePlateLook;
    }

    void Start()
    {
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
                GridFactory.GetInstance().AddPlate(this); // the zonemanager should handle this, nod grid factory
            }
        }
        this.gameObject.SetActive(plateState);
    }


}
