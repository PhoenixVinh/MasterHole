using _Scripts.UI.MissionUI;
using UnityEngine;
using UnityEngine.UIElements;

namespace _Scripts.ObjectPooling
{
    public class MissionPooling : ObjectPoolingBase<MissionPooling>
    {
        public GameObject spawnImage()
        {
            GameObject obj = this.GetPooledObject();
            
            return obj;
        }

        public void DisactiveAllItem()
        {
            foreach (var poolObject in poolObjects)
            {
                poolObject.gameObject.SetActive(false);
            }
        }
    }
}