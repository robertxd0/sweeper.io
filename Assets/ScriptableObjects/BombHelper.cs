using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create bomb helper instance", fileName = "BombHelperInstance")]
public class BombHelper : ScriptableObject
{
    public static ObjectPooler bombPool;

    public GameObject bombPrefab = null;

    public void InitHelper()
    {
        //init pooler
        ObjectPooler pool = new ObjectPooler(25, bombPrefab);
        bombPool = pool;
    }

    public Bomb SpawnBomb()
    {          
        var bomb = bombPool.GetObject();
        Bomb bombObj = bomb.GetComponent<Bomb>();

        return bombObj;
    }
}