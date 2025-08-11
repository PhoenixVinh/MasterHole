using System;
using UnityEngine;


public class ItemIgnore : MonoBehaviour
{


    private Collider _collider;
    public void Awake()
    {
        _collider = GetComponent<Collider>();   
    }

   

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == this.gameObject.name)
        {
            Physics.IgnoreCollision(other.collider,_collider ,true);
        }
    }
}
