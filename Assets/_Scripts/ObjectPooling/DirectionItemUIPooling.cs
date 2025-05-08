using System;
using UnityEngine;

namespace _Scripts.ObjectPooling
{
    public class DirectionItemUIPooling : ObjectPoolingBase<DirectionItemUIPooling>
    {
        public override GameObject GetPooledObject()
        {
            GameObject newObj = Instantiate(objectPoolingPrefab, transform);
            newObj.name = objectPoolingPrefab.name;
            poolObjects.Add(newObj);
            return newObj;
        }
        
        
        
    }
}