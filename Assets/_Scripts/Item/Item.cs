using System;
using System.Collections;
using _Scripts.Event;
using _Scripts.Hole;
using _Scripts.Map.MapSpawnItem;
using _Scripts.ObjectPooling;
using _Scripts.Sound;
using _Scripts.UI.MissionUI;
using _Scripts.Vibration;
using Unity.VisualScripting;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int score  = 1;
    public string type = "food";
    private Rigidbody rb;
    private bool isGetScore = false;
    public void SetData(string foodName, int score)
    {
        this.score = score;
        this.type = foodName;
        rb = GetComponent<Rigidbody>();
        isGetScore = false;
    }
    
   
    
    
    

    private void OnTriggerEnter(Collider other)
    {
    
        
        
        
        
        if (!other.CompareTag("HoleBottom") || isGetScore) return;
        
        if (ManagerSound.Instance != null)
        {
            ManagerSound.Instance.PlayEffectSound(EnumEffectSound.EatItem);
        }
    
        if (ManagerVibration.Instance != null)
        {
            ManagerVibration.Instance.UseVibration(EnumVibration.Light);
        }
    
       
                 
        isGetScore = true;      
        ItemEvent.OnAddScore?.Invoke(score);
        SpawnItemMap.Instance.RemoveItem(gameObject);
        TextPooling.Instance.SpawnText(HoleController.Instance.transform.position + Vector3.up * 2, score);
                
        ManagerMission.Instance.CheckMinusItems(gameObject.name);
        
        
        StartCoroutine(DestroyCoroutine());
    
    
    }

    
    private IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        if (gameObject != null){
            rb.isKinematic = true;
            rb.useGravity = false;
            CleanGradeManager.Instance?.AddObject(gameObject);
        }
        
               
    }

    
}