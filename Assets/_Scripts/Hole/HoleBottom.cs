
using _Scripts.Event;
using _Scripts.Map.MapSpawnItem;
using _Scripts.ObjectPooling;
using _Scripts.Sound;
using _Scripts.UI.MissionUI;
using UnityEngine;

namespace _Scripts.Hole
{
    public class HoleBottom : MonoBehaviour
    {
        private void OnTriggerStay(Collider other)
        {
            // Check if it is the item Destroy it 
            if (other.CompareTag("Item"))
            {

//                ManagerSound.Instance.PlayEffectSound(EnumEffectSound.EatItem);
                int score = other.transform.parent.GetComponent<Item>().score;
                ItemEvent.OnAddScore?.Invoke(score);
                SpawnItemMap.Instance.RemoveItem(other.gameObject);
                TextPooling.Instance.SpawnText(HoleController.Instance.transform.position + Vector3.up * 2, score);
                
                ManagerMission.Instance.CheckMinusItems(other.transform.parent.GetComponent<Item>().type, other.transform.position);
                Destroy(other.transform.parent.gameObject);
                
               
            }
            
        }
    }
}