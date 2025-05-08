using UnityEngine;

namespace _Scripts.ObjectPooling
{
    public class HightLightItemUIPooling : ObjectPoolingBase<HightLightItemUIPooling>
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