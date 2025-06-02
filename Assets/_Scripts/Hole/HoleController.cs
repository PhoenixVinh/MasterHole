using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using _Scripts.Effects;
using _Scripts.Event;
using _Scripts.Hole;
using _Scripts.UI;
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
 
    [Header("Effects")] public HoleScaleEffect holeScaleEffect;

    
    
    private HoleMovement _holeMovement;
    public HoleMovement HoleMovement => _holeMovement;

    //public BlackHole _blackHole;
   // public List<GameObject> bottomHoles;

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
                .Append(transform.DOScale(new Vector3(radius * 1.3f, localScale.y, radius*1.3f), 0.1f))
                .Append(transform.DOScale(newScale, 0.2f));
                
            CameraFOVEvent.OnLevelUpEvent?.Invoke(0.3f);
        }
        else
        {
            transform.localScale = newScale;
        }
        HoleEvent.OnUpdateFade?.Invoke(radius);
        // if (indexSkin != 1)
        // {
        //     HoleSkins.transform.localScale = new Vector3(1, 1, radius);
        // }
        
        

        
       
        
        this._holeLevel.SetData(amountExp);
    }

    public async void Upscale(float time)
    {

        float radious = GetCurrentRadius() * 1.5f;
        Vector3 localScale = transform.localScale;
        Vector3 newScale = new Vector3(radious, localScale.y, radious);
        
        //transform.localScale = newScale;
        DOTween.Sequence()
            .SetId("HoleUpScale")
            .Append(transform.DOScale(new Vector3(radious * 1.3f, localScale.y, radious*1.3f), 0.1f))
            .Append(transform.DOScale(newScale, 0.2f));
        // if (indexSkin != 1)
        // {
        //     HoleSkins.transform.localScale = new Vector3(1, 1, radious);
        // }
        
        await Task.Delay((int)time*10000);
        transform.localScale = new Vector3(GetCurrentRadius(), localScale.y, GetCurrentRadius());
        //CameraFOVEvent.OnLevelUpEvent?.Invoke(0.3f);
        
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
