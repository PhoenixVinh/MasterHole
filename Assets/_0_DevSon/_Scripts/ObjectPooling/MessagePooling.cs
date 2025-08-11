using TMPro;
using UnityEngine;

namespace _Scripts.ObjectPooling
{
    public class MessagePooling : ObjectPoolingBase<MessagePooling>
    {
        public void SpawnMessage(Vector3 position, string content)
        {

            GameObject message = this.GetPooledObject();
           
            message.SetActive(false);
            message.GetComponent<TMP_Text>().text = content;
            message.transform.position = position;
            message.SetActive(true);
        }
        
        
    }
}