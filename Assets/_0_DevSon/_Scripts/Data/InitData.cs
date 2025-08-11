using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Booster;
using _Scripts.UI;
using UnityEngine;

public class InitData : MonoBehaviour
{
    public void Awake()
    {
        
       
        
        
    }



    public void InItData()
    {
        if (!PlayerPrefs.HasKey(StringPlayerPrefs.INIT_DATA))
        {
            
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt(StringPlayerPrefs.INIT_DATA, 1);
            
            // set data default
            
            
            PlayerPrefs.SetInt(StringPlayerPrefs.TUTORIAL_LEVEL_3, 0);
            PlayerPrefs.SetInt(StringPlayerPrefs.TUTORIAL_LEVEL_5, 0);
            PlayerPrefs.SetInt(StringPlayerPrefs.TUTORIAL_LEVEL_7, 0);
            PlayerPrefs.SetInt(StringPlayerPrefs.TUTORIAL_LEVEL_9, 0);
            PlayerPrefs.SetInt(StringPlayerPrefs.TUTORIAL_SKIN_4, 0);
            
            PlayerPrefs.SetString(StringPlayerPrefs.UNLIMITED_TIME, DateTime.Now.ToString());
            PlayerPrefs.SetInt(StringPlayerPrefs.REMOVED_ADS_PACK, 0);
            PlayerPrefs.SetInt(StringPlayerPrefs.STARTER_DEAL_PACK, 0);
            AddNewValue();
          
            PlayerPrefs.SetInt(StringPlayerPrefs.CURRENT_LEVEL, 1);
            PlayerPrefs.SetInt(StringPlayerPrefs.CURRENT_ENERGY, 5);
            PlayerPrefs.SetInt(StringPlayerPrefs.CURRENT_COIN, 900);
            
            
            
            Debug.Log("Init Data");
            
            
        }
    }
    
    private void AddNewValue()
    {
        
        PlayerPrefs.SetString(StringPlayerPrefs.BOOSTER_DATA, "");
        var boosterDatas = new BoosterDatas();
        boosterDatas.Boosters = new List<BoosterData>();
       
       
            
        boosterDatas.Boosters.Add(new BoosterData
        {
            Name = "B_scale",
            Amount = 0,
            LevelCanUse = 3,
        });
        boosterDatas.Boosters.Add(new BoosterData
        {
            Name = "B_Magnet",
            Amount = 0,
            LevelCanUse = 5,
        });
        boosterDatas.Boosters.Add(new BoosterData
        {
            Name = "B_Location",
            Amount = 0,
            LevelCanUse = 7,
        });
        boosterDatas.Boosters.Add(new BoosterData
        {
            Name = "B_Ice",
            Amount = 0,
            LevelCanUse = 9,
        });
        
        
        String convert = JsonUtility.ToJson(boosterDatas);
        PlayerPrefs.SetString(StringPlayerPrefs.BOOSTER_DATA, convert);
       
            
    }
}
