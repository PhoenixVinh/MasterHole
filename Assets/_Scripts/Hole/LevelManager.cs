using _Scripts.Sound;
using _Scripts.UI;
using HoleLevelData;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // it can load by recouses
    public LevelHoleSO datalevel;

    private int currentLevel = 0;


    public int CurrentLevel
    {
        get { return currentLevel; }
        private set { currentLevel = value; }
    } 
    public void Start()
    {
        UpdateLevel();
        currentLevel = 0;
    }

    private void OnEnable()
    {
        currentLevel = 0;
        HoleEvent.OnLevelUp += OnLevelup;
    }

    private void OnLevelup()
    {
        currentLevel++;
        if (currentLevel < datalevel.levels.Length)
        {
            UpdateLevel();
        }
        else
        {
            Debug.Log("No more levels");
        }
       
    }

    private void OnDisable()
    {
        HoleEvent.OnLevelUp -= OnLevelup;
    }

    private void UpdateLevel()
    {
        var dataLevel = datalevel.levels[currentLevel];
        int expUpLevel = 0;
        if (currentLevel + 1 < datalevel.levels.Length)
        {
            expUpLevel = datalevel.levels[currentLevel + 1].amountExp;
        }
        else
        {
            expUpLevel = 100000;
        }

        if (currentLevel == 0)
        {
            HoleController.Instance.LoadLevel(expUpLevel, dataLevel.radious, false);
        }
        else
        {
            if (ManagerSound.Instance != null)
            {
                ManagerSound.Instance.PlayEffectSound(EnumEffectSound.LevelUpHole);
            }
            HoleController.Instance.PlayHoleScaleUp();
            HoleController.Instance.LoadLevel(expUpLevel, dataLevel.radious, true);
            //HoleController.Instance.UpScaleAnim(dataLevel.radious);
            
        }
       
       

    }

    public void ResetLevel()
    {
        currentLevel = 0; 
        UpdateLevel();
    }

    public float GetScalelevel()
    {
        return datalevel.levels[currentLevel].radious;
    }
    
    
    
    
    
    
    
}