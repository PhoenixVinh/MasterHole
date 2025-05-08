using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Effects;
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


    public void OnUpLevelHole()
    {
       // this._blackHole.changeInitialScale(this.transform.localScale.x);
    }


    public void LoadLevel(int amountExp, float radius, bool isAnim)
    {
        Vector3 localScale = transform.localScale;
        Vector3 newScale = new Vector3(radius, localScale.y, radius);
        // Update Scale of Hole 
        if (isAnim)
        {
            DOTween.Sequence()
                .SetId("HoleUpScale")
                .Append(transform.DOScale(newScale, 1f))
                .OnComplete(() => OnUpLevelHole());
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
        return _levelManager.currentLevel;
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
        Vector3 scaleUp = new Vector3(dataLevelRadious, transform.localScale.y, dataLevelRadious);
        
        
        DOTween.Sequence().SetId("HoleAnim")
            .Append(transform.DOScale(scaleUp*1.5f,0.3f))
            .Append(transform.DOScale(scaleUp,0.3f));
        

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
