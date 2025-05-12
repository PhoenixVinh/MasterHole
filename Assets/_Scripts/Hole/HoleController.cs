using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Effects;
using _Scripts.Event;
using _Scripts.Hole;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;


public enum SpecialSkill
{
    IncreaseRange = 0, 
    Magnet = 1,
    Direction = 2, 
    FreezeColdown = 3,
}


public class HoleController : MonoBehaviour
{
    public static HoleController Instance;
    
    [Header("Variables")]
    public float _speedMovement;
    public float _radious;
    [Header("Effects")] public HoleScaleEffect holeScaleEffect;

    
    
    private HoleMovement _holeMovement;
    public HoleMovement HoleMovement => _holeMovement;

    //public BlackHole _blackHole;


    private HoleLevel _holeLevel;
    public HoleLevel HoleLevel => _holeLevel;


    private HoleSpecialSkill _holeSpecialSkill;
    [SerializeField] private LevelManager _levelManager;
    
    private void Awake()
    {
        Instance = this;
        holeScaleEffect.gameObject.SetActive(false);
        _holeMovement = GetComponent<HoleMovement>();
        //_blackHole = GetComponent<BlackHole>();
        _holeLevel = GetComponent<HoleLevel>();
        _holeSpecialSkill = GetComponent<HoleSpecialSkill>();
        SetData();
        
    }

    

    private void SetData()
    {
        _holeMovement.SetSpeedMovement(_speedMovement);
    }





    public void LoadLevel(int amountExp, float radius, bool isAnim)
    {
        Vector3 localScale = transform.localScale;
        Vector3 newScale = new Vector3(radius, localScale.y, radius);
        // Update Scale of Hole 
        if (isAnim)
        {
            
            //transform.localScale = newScale;
            DOTween.Sequence()
                .SetId("HoleUpScale")
                .Append(transform.DOScale(new Vector3(radius * 0.7f, localScale.y, radius*0.7f), 0.1f))
                .Append(transform.DOScale(newScale, 0.3f));
                
            CameraFOVEvent.OnLevelUpEvent?.Invoke(0.4f);
        }
        else
        {
            transform.localScale = newScale;
        }
       

        
       
        
        this._holeLevel.SetData(amountExp);
    }
    
    
    
    
    
    
    
    

    public void ProcessSkill(int index)
    {
        SpecialSkill skill = (SpecialSkill)index;
        this._holeSpecialSkill.ProcessSkill(skill);
    }

    public float GetCurrentScale()
    {
        return this.transform.localScale.x; 
    }

    public float GetCurrentRadius()
    {
        return _levelManager.GetScalelevel();
    }

    public int GetCurrentLevel()
    {
        return _levelManager.CurrentLevel;
    }
    // private  IncreaseRangeCoroutine()
    // {
    //     float timeIncrease = 20f; 
    // }
    public void SetPosition(Vector3 position)
    {
        this.transform.position = position;
        this._holeSpecialSkill.StopEventSkill();
        //this._levelManager.ResetLevel();
        
    }

    public void Reset()
    {
        this._levelManager.ResetLevel();
    }

    public bool IsProcessSkill(int index)
    {
        return this._holeSpecialSkill.UsingSkill(index);
    }

    public void UpScaleAnim(float dataLevelRadious)
    {
        Vector3 scaleUp = new Vector3(dataLevelRadious*1.25f, transform.localScale.y, dataLevelRadious*1.25f);
        
        
        
        
        DOTween.Sequence().SetId("HoleAnim")
            .Append(transform.DOScale(scaleUp,0.3f))
            .Append(transform.DOScale(new Vector3(dataLevelRadious,transform.localScale.y, dataLevelRadious ),0.3f));
        

    }


    public void PlayHoleScaleUp()
    {
        holeScaleEffect.gameObject.SetActive(true);
        holeScaleEffect.PlayEffect(transform.localScale.x);
    }
    public void OnDestroy()
    {
        DOTween.Kill("HoleUpScale");
        DOTween.Kill("HoleAnim");
    }


   
}
