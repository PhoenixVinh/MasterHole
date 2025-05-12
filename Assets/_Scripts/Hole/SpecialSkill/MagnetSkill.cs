using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _Scripts.Sound;
using DG.Tweening;
using DG.Tweening.Core;
using Unity.VisualScripting;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;


public struct suckecObjectParameter
{
    public Vector3 originScale;
    public bool isSuction;

    public suckecObjectParameter(Vector3 originScale, bool isSuction)
    {
        this.originScale = originScale;
        this.isSuction = isSuction;
    }

    public void SetBoolSuction(bool isSuction)
    {
        this.isSuction = isSuction; 
    }
}

public class MagnetSkill : MonoBehaviour
{
    
    
    public float addingRadius = 1.25f;
    public float suctionforce = 1f;
    Dictionary<GameObject, suckecObjectParameter> _suckecObjects = new Dictionary<GameObject, suckecObjectParameter>();
   
    
    
    private SphereCollider _collider;

    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
        
    }

    private void OnEnable()
    {
        _collider.radius = 0.5f + addingRadius/10f;
        
    }
    
    
    private void Start()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (!_suckecObjects.ContainsKey(other.gameObject))
        {
            _suckecObjects.Add(other.gameObject, new suckecObjectParameter(other.gameObject.transform.localScale, false) );
        }
    }
    
    
    


    private void FixedUpdate()
    {

        float radious = _collider.radius * HoleController.Instance.GetCurrentScale();
        
        // Get radious of Circle 
        
        // Pull All Item in the list to the Hole 
        foreach (var item in _suckecObjects)
        {

            Vector3 pos = HoleController.Instance.transform.position;
            GameObject obj = item.Key;
            // Move Object to the Hole 
            if(obj == null) continue;
            if(item.Value.isSuction) continue;
            
            float distance = Vector3.Distance(pos, obj.transform.position);
            if (distance > radious + 0.2f)
            {
                if(item.Value.isSuction) continue;
                else
                {
                    if (obj.transform.localScale.x < item.Value.originScale.x)
                    {
                        //obj.transform.localScale /= 0.95f;
                    }
                    continue;
                }
                
            }
            if (distance < 0.2f)
            {
                item.Value.SetBoolSuction(true);
                
                continue;
            }
            
          

            
            // Check distance of 
            Vector3 directionMovement = (pos - obj.transform.position).normalized;
            // if(obj.layer == LayerMask.NameToLayer(LayerMaskVariable.NoCollision.ToString()))
            // {
            //     continue;
            // }
            //
            obj.transform.position += directionMovement * suctionforce * Time.deltaTime;
            // Scale object to the Hole 
            // Check if it is scaled => Don't Scale again 
            // Scale Object
            // Vector3 minSacle = item.Value.originScale/2f;
            // if (obj.transform.localScale.x > minSacle.x)
            // {
            //     obj.transform.localScale *= 0.95f;
            // }
        }
        
    }

    public async void OnDisable()
    {
        foreach (var item in _suckecObjects)
        {
            if(item.Key == null) continue;
            else
            {
                
                while ( item.Key != null && item.Key.transform.localScale.x < item.Value.originScale.x) 
                {
                    item.Key.transform.localScale /= 0.95f;
                    await Task.Delay(50);
                }
            }
        }
      
    }
}