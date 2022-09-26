using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonPlate : MonoBehaviour
{
    [SerializeField] Material triggerablePlateLook;

    internal bool isTriggered;


    private void OnTriggerEnter(Collider other) {
        if(other.tag == Constants.TAG_PLAYER)
        {

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
        foreach(Collider col in overlappingColliders)
        {
            if(col.tag == Constants.TAG_PLAYER)
            {
                plateState = true;
            }
        }
        this.gameObject.SetActive(plateState);
    }


}
