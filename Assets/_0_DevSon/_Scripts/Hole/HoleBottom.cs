
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

        
        private void OnTriggerEnter(Collider other)
        {


            if (other.CompareTag("Item") && !Items.Contains(other.transform.parent.gameObject))
            {

                //Debug.Log(other.transform.parent.name);

                if (ManagerSound.Instance != null)
                {
                    ManagerSound.Instance.PlayEffectSound(EnumEffectSound.EatItem);
                }

                if (ManagerVibration.Instance != null)
                {
                    ManagerVibration.Instance.UseVibration(EnumVibration.Light);
                }
               

                var item = other.GetComponentInParent<Item>();
                item.DestroyObject();
                Items.Add(other.transform.parent.gameObject);
                

              

                

            }
            
        }
        
        
        
        public void OnDestroy()
        {
            StopAllCoroutines();
            Items.Clear();
        }
    }
}