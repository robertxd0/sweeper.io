using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class provides the hexagonal tiles with bombs.
public class BombFactory : MonoBehaviour
{
    [SerializeField] private BombHelper bombHelper;

    private void OnEnable()
    {
        HexagonPlate.PlateTriggeredEvent.AddListener(PlateTriggeredEvent);
    }

    private void OnDisable()
    {
        HexagonPlate.PlateTriggeredEvent.RemoveListener(PlateTriggeredEvent);
    }

    void Start()
    {
        bombHelper.InitHelper();
    }

    void PlateTriggeredEvent(HexagonPlate plate)
    {
        if(!plate.hasBomb)
        {
            var bomb = bombHelper.SpawnBomb();
            var pos = plate.transform.position;
            pos.y += 1f;

            bomb.SetPlate(plate);
            bomb.gameObject.transform.position = pos;
        }
    }

}
