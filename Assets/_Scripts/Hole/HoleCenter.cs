
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public class HoleCenter : MonoBehaviour
{
  
    private Dictionary<GameObject, Item> ListObjects = new Dictionary<GameObject, Item>(); // Hàng đợi lưu Rigidbody 
 
    
    private void OnTriggerEnter(Collider other)
    {

        other.transform.parent.transform.Translate(Vector3.down*0.0001f);
        if (other.CompareTag("Item") && !ListObjects.ContainsKey(other.transform.parent.gameObject))
        {
            // var rb = other.transform.parent.GetComponent<Rigidbody>();
            // if (rb == null)
            // {
            //     rb = other.GetComponent<Rigidbody>();
            // }
            //rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            var Item = other.GetComponentInParent<Item>();
            ListObjects.Add(other.transform.parent.gameObject, Item);
            Item.SetPhysic();
            //rb.isKinematic = false;
            //StartCoroutine(ProcessQueue());
          
        }

        ListObjects[other.transform.parent.gameObject].SetWakeUpPhysic();
    }

 
        
  


    // IEnumerator ProcessQueue()
    // {
    //    
    // }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Item") && ListObjects.ContainsKey(other.transform.parent.gameObject))
        {
            other.gameObject.layer = LayerMask.NameToLayer("Collision");
            other.transform.parent.gameObject.layer = LayerMask.NameToLayer("Collision");
            ListObjects.Remove(other.transform.parent.gameObject);
        }
    }
}