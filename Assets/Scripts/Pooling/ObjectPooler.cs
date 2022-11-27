using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : IEnumerable
{
	private Stack<GameObject> objectPool =  new Stack<GameObject>();
	private GameObject prefab;


	public ObjectPooler(int poolSize, GameObject prefab)
	{
		this.prefab = prefab;
		this.ExpandPool(poolSize);
	}

    public IEnumerator GetEnumerator()
    {
        foreach(GameObject obj in objectPool)
		{
			yield return obj;
		}
    }

    public GameObject GetObject()
	{
		if(objectPool.Count == 0)
		{
			this.ExpandPool(1);
		}
		
		GameObject obj = objectPool.Pop();
		obj.SetActive(true);

		return obj;
	}


	public void PoolObject(GameObject objToPool)
	{
		this.objectPool.Push(objToPool);
	}

	private void ExpandPool(int count)
	{
		for(int i = 0; i < count; i++)
		{
			GameObject instance = GameObject.Instantiate(prefab);
			instance.SetActive(false);
			objectPool.Push(instance);
		}	
	}


}