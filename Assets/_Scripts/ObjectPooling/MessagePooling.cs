using UnityEngine;

namespace _Scripts.ObjectPooling
{
    public class MessagePooling : ObjectPoolingBase<MessagePooling>
    {
        public void SpawnMessage(Vector3 position)
        {
           
            GameObject message = this.GetPooledObject();
            message.SetActive(false);
            message.transform.position = position;
            message.SetActive(true);
        }
    }
}