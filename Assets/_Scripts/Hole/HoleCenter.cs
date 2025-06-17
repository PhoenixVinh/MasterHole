
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum LayerMaskVariable
{
    Collision,
    NoCollision,
}


public class HoleCenter : MonoBehaviour
{
  
    private Queue<Item> ItemQueue = new Queue<Item>(); // Hàng đợi lưu Rigidbody 
    private float timer = 0.02f;
    
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            // var rb = other.transform.parent.GetComponent<Rigidbody>();
            // if (rb == null)
            // {
            //     rb = other.GetComponent<Rigidbody>();
            // }
            //rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            var Item = other.GetComponentInParent<Item>();
            //ItemQueue.Enqueue(Item);
         
            //other.transform.parent.Translate(Vector3.down*0.0001f);
            other.transform.parent.gameObject.layer = LayerMask.NameToLayer(LayerMaskVariable.NoCollision.ToString());
            other.gameObject.layer = LayerMask.NameToLayer(LayerMaskVariable.NoCollision.ToString());
            Item.SetPhysic();
            //rb.isKinematic = false;
            //StartCoroutine(ProcessQueue());
          
        }
    }

    private void FixedUpdate()
    {
        if (ItemQueue.Count > 0 && timer > 0.02)
        {

            for (int i = 0; i <= 5; i++)
            {
                if (ItemQueue.Count > 0)
                {
                    var rb = ItemQueue.Dequeue();
                    if (rb != null)
                    {
                        rb.SetPhysic();
                    }
                }

                break;
                // Tối ưu hóa Rigidbody

            }
                
          
            timer = 0;

        }
        else
        {
            timer += Time.fixedDeltaTime;
        }
        
    }


    // IEnumerator ProcessQueue()
    // {
    //    
    // }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            other.gameObject.layer = LayerMask.NameToLayer(LayerMaskVariable.Collision.ToString());
            other.transform.parent.gameObject.layer = LayerMask.NameToLayer(LayerMaskVariable.Collision.ToString());
        }
    }
}