using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create bomb helper instance", fileName = "BombHelperInstance")]
public class BombHelper : ScriptableObject
{
    public GameObject bombPrefab = null;

    public void InitHelper()
    {
        //init?  
    }

    public GameObject SpawnBomb()
    {                       
        return Instantiate(bombPrefab);
    }
}