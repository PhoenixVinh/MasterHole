
using System.Collections;

using UnityEngine;



public enum LayerMaskVariable
{
    Collision,
    NoCollision,
}


public class HoleCenter : MonoBehaviour
{
  
 

    
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            var rb = other.transform.parent.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = other.GetComponent<Rigidbody>();
            }
            //rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            
            other.transform.parent.Translate(Vector3.down*0.0001f);
            other.transform.parent.gameObject.layer = LayerMask.NameToLayer(LayerMaskVariable.NoCollision.ToString());
            other.gameObject.layer = LayerMask.NameToLayer(LayerMaskVariable.NoCollision.ToString());
            rb.isKinematic = false;
          
        }
    }
    

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            other.gameObject.layer = LayerMask.NameToLayer(LayerMaskVariable.Collision.ToString());
            other.transform.parent.gameObject.layer = LayerMask.NameToLayer(LayerMaskVariable.Collision.ToString());
        }
    }
}