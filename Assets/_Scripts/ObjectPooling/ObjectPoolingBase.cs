using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingBase<T> : MonoBehaviour where T : Component
{

    public static T Instance;

    public virtual void Awake()
    {
        if(Instance == null)
            Instance = this as T;
        else
        {
            Destroy(this);
        }
    }


    //Using addressable
    public GameObject objectPoolingPrefab;
    public List<GameObject> poolObjects;
    
    
    
    public virtual GameObject GetPooledObject()
    {
        foreach (GameObject obj in poolObjects)
        {
            if (!obj.activeInHierarchy)
            {
                return obj;
            }
        }
        
        GameObject newObj = Instantiate(objectPoolingPrefab, transform);
        newObj.name = objectPoolingPrefab.name;
        poolObjects.Add(newObj);
        return newObj;
    }
    
   

    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        
    }
    
    public int GetCurrentActiveCount()
    {
        int count = 0;
        foreach (GameObject obj in poolObjects)
        {
            if (obj.activeInHierarchy)
            {
                count++;
            }
        }
        return count;
    }
}