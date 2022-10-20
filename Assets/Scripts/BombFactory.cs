using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombFactory : MonoBehaviour
{

    private void OnEnable() {
        HexagonPlate.PlateTriggeredEvent.AddListener(PlateTriggered);
    }

    private void OnDisable() {
        HexagonPlate.PlateTriggeredEvent.RemoveListener(PlateTriggered);
    }

    void Start()
    {
        
    }

    void PlateTriggered(HexagonPlate plate)
    {
        Debug.Log("Factory recorded trigger: " + plate.gameObject.name);
    }

}
