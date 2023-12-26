using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : SingleTone<ObjectPool>
{
    private Dictionary<GameObject, LinkedStack<GameObject>> pool = new Dictionary<GameObject, LinkedStack<GameObject>>();
    private GameObject saveObj;
    public void  OutObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (!pool.ContainsKey(prefab))
        {
            LinkedStack<GameObject> SaveStack = new LinkedStack<GameObject>();
            pool.Add(prefab, SaveStack);
        }
        if (pool[prefab].Count().Equals(0))
        {
            saveObj = Instantiate(prefab);
        }
        else
        {
            saveObj = pool[prefab].Push();
            saveObj.SetActive(true);
        }
        saveObj.transform.SetPositionAndRotation(position, rotation);
    }

    public void InObject(GameObject prefab, GameObject bfInObj)
    {
        if (pool.ContainsKey(prefab))
        {
            bfInObj.SetActive(false);
            pool[prefab].Add(bfInObj);
        }

    }
}

