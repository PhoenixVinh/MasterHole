using System;
using System.Collections;
using System.Threading.Tasks;
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
    
    private bool isPhysic = false;
    
    
    [SerializeField]private string nameLayerOn = "NoCollision";
    [SerializeField]private string nameLayerOff = "Collision";
    public void SetData(string foodName)
    {
      
        this.type = foodName;
        rb = GetComponent<Rigidbody>();
        isGetScore = false;
    }


    public void SetPhysic()
    {
        
        if (!isPhysic)
        {
            rb.isKinematic = false;
            isPhysic = true;
           
            SetLayerOn();
          
        
            
        }
        SetLayerOn();
        rb.WakeUp();

        
        
            
        //rb.velocity = new Vector3(0, -0.1f, 0);
        // if (isPhysic) return;
        // StartCoroutine(FallSmoothly());
    }


    public void SetWakeUpPhysic()
    {
        transform.Translate(Vector3.down*0.0001f);
        rb.WakeUp();
    }
    
    
    
    
    



    public void DestroyObject()
    {
        isGetScore = true;      
        ItemEvent.OnAddScore?.Invoke(score);
        //SpawnItemMap.Instance.RemoveItem(gameObject);
        TextPooling.Instance.SpawnText(HoleController.Instance.transform.position + Vector3.up * 2, score);
                
        ManagerMission.Instance.CheckMinusItems(gameObject.name, gameObject);
        
        // rb.isKinematic = true;
        // rb.useGravity = false;
        //this.gameObject.SetActive(false);
        StartCoroutine(DestroyCoroutine());
    }

    
    private IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        if (gameObject != null){
            rb.isKinematic = true;
            rb.useGravity = false;
            DestroyObject(gameObject);
        }
        
               
    }

    public void SetLayerOn()
    {
   
        gameObject.layer = LayerMask.NameToLayer(nameLayerOn);
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer(nameLayerOn);
        }
    }

    public void SetLayerOff()
    {
        gameObject.layer = LayerMask.NameToLayer(nameLayerOff);
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer(nameLayerOff);
        }
    }

    
}