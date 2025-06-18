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
    
    public bool isPhysic = false;
    public void SetData(string foodName, int score)
    {
        this.score = score;
        this.type = foodName;
        rb = GetComponent<Rigidbody>();
        isGetScore = false;
    }


    public void SetPhysic()
    {
        //transform.Translate(Vector3.down*0.0001f);
        if (!isPhysic)
        {
            rb.isKinematic = false;
            isPhysic = true;
           
          
          
            // for (int i = 0; i < transform.childCount; i++)
            // {
            //     transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer(LayerMaskVariable.NoCollision.ToString());
            // }
            // transform.gameObject.layer = LayerMask.NameToLayer(LayerMaskVariable.NoCollision.ToString());
            //other.gameObject.layer = LayerMask.NameToLayer(LayerMaskVariable.NoCollision.ToString());
            
        }
        gameObject.layer = LayerMask.NameToLayer("NoCollision");
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer("NoCollision");
        }
        rb.WakeUp();

        
        
            
        //rb.velocity = new Vector3(0, -0.1f, 0);
        // if (isPhysic) return;
        // StartCoroutine(FallSmoothly());
    }


    public void SetWakeUpPhysic()
    {
        rb.WakeUp();
    }
    // private IEnumerator FallSmoothly()
    // {
    //     while (!isPhysic)
    //     {
    //         // Di chuyển mượt mà dựa trên thời gian thực
    //         transform.position -= new Vector3(0, 10* Time.deltaTime, 0);
    //
    //         // Kiểm tra vị trí Y
    //         if (transform.position.y <= 0.1)
    //         {
    //             isPhysic = true;
    //             rb.isKinematic = false; // Tắt kinematic khi chạm ngưỡng
    //         }
    //
    //         yield return null; // Chờ đến khung hình tiếp theo
    //     }
    // }
    
    
    
    

    // private void OnTriggerEnter(Collider other)
    // {
    //
    //     
    //     Debug.Log(other.transform.parent.name);
    //     
    //     
    //     if (!other.CompareTag("HoleBottom") || isGetScore) return;
    //     
    //     
    //
    //
    // }

    public void DestroyObject()
    {
        isGetScore = true;      
        ItemEvent.OnAddScore?.Invoke(score);
        //SpawnItemMap.Instance.RemoveItem(gameObject);
        //TextPooling.Instance.SpawnText(HoleController.Instance.transform.position + Vector3.up * 2, score);
                
        ManagerMission.Instance.CheckMinusItems(gameObject.name, gameObject);
        
        rb.isKinematic = true;
        rb.useGravity = false;
        this.gameObject.SetActive(false);
        //StartCoroutine(DestroyCoroutine());
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