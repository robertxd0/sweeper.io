using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombFactory : MonoBehaviour
{
    [SerializeField] BombHelper bombHelper;

    private void OnEnable()
    {
        HexagonPlate.PlateTriggeredEvent.AddListener(PlateTriggered);
    }

    private void OnDisable()
    {
        HexagonPlate.PlateTriggeredEvent.RemoveListener(PlateTriggered);
    }

    void Start()
    {

    }

    void PlateTriggered(HexagonPlate plate)
    {
        var bomb = bombHelper.SpawnBomb();
        var pos = plate.transform.position;
        pos.y += 1f;

        bomb.SetPlate(plate);
        bomb.gameObject.transform.position = pos;
    }

}
