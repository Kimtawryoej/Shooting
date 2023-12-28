using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : SingleTone<ObjectPool>
{
    private Dictionary<LayerMask, LinkedStack<GameObject>> pool = new Dictionary<LayerMask, LinkedStack<GameObject>>();
    private GameObject saveObj;
    public GameObject  OutObject(LayerMask layer, GameObject prefab,Vector3 position, Quaternion rotation)
    {
        if (!pool.ContainsKey(layer))
        {
            LinkedStack<GameObject> SaveStack = new LinkedStack<GameObject>();
            pool.Add(layer, SaveStack);
        }
        if (pool[layer].Count().Equals(0))
        {
            saveObj = Instantiate(prefab);
        }
        else
        {
            saveObj = pool[layer].Push();
            saveObj.SetActive(true);
        }
        saveObj.transform.SetPositionAndRotation(position, rotation);
        return saveObj;
    }

    public void InObject(LayerMask prefab, GameObject bfInObj)
    {
        if (pool.ContainsKey(prefab))
        {
            bfInObj.SetActive(false);
            pool[prefab].Add(bfInObj);
        }
    }
}

