
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
        
            if (other.CompareTag("Item") && !Items.Contains(other.gameObject))
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

                int score = 1;
                try
                {
                    score = other.transform.parent.GetComponent<Item>().score;
                }
                catch(Exception e)
                {
                    Debug.LogError(other.name + " has no score");
                }
                 
               
                ItemEvent.OnAddScore?.Invoke(score);
                SpawnItemMap.Instance.RemoveItem(other.gameObject);
                TextPooling.Instance.SpawnText(HoleController.Instance.transform.position + Vector3.up * 2, score);
                
                ManagerMission.Instance.CheckMinusItems(other.transform.parent.name);
                Items.Add(other.gameObject);

                StartCoroutine(DestroyCoroutine(other.transform.gameObject));
                
               
                
                
               
            }
            
        }

        private IEnumerator DestroyCoroutine(GameObject transformGameObject)
        {
            yield return new WaitForSeconds(0.5f);

            if (transformGameObject != null)
            {
                Items.Remove(transformGameObject);
                Destroy(transformGameObject.transform.parent.gameObject);
            }
               
        }

        public void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
}