
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using _Scripts.Event;
using _Scripts.Map.MapSpawnItem;
using _Scripts.ObjectPooling;
using _Scripts.Sound;
using _Scripts.UI.MissionUI;
using _Scripts.Vibration;
using UnityEngine;

namespace _Scripts.Hole
{
    public class HoleBottom : MonoBehaviour
    {
        public HashSet<GameObject> Items;
            
        private void OnEnable()
        {
            Items = new HashSet<GameObject>();
        }

        
        // private void OnTriggerEnter(Collider other)
        // {
        //
        //     if (other.CompareTag("Item"))
        //     {
        //         
        //         //Debug.Log(other.transform.parent.name);
        //        
        //
        //       
        //         
        //         
        //        
        //     }
        //     
        // }
        //
        //
        //
        // public void OnDestroy()
        // {
        //     StopAllCoroutines();
        // }
    }
}