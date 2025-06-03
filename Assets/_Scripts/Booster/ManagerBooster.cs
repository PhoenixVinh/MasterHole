using System;
using System.Collections.Generic;
using _Scripts.UI;
using _Scripts.UI.BoosterUI;
using Unity.VisualScripting;
using UnityEngine;

namespace _Scripts.Booster
{
    public class ManagerBooster : MonoBehaviour
    {
        
        public static ManagerBooster Instance;
        public BoosterDatas boosterDatas;
       

        [Header("Booster UI")] 
        public List<BoosterUI> BoostersUI;


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            
           
            if(PlayerPrefs.HasKey(StringPlayerPrefs.BOOSTER_DATA))
                boosterDatas = JsonUtility.FromJson<BoosterDatas>(PlayerPrefs.GetString(StringPlayerPrefs.BOOSTER_DATA));
                
            else
            {
                boosterDatas = new BoosterDatas();
                boosterDatas.Boosters = new List<BoosterData>();
                AddNewValue();
                String convert = JsonUtility.ToJson(boosterDatas);
                PlayerPrefs.SetString(StringPlayerPrefs.BOOSTER_DATA, convert);
            }
            SaveBoosterData();
            
        }

        private void AddNewValue()
        {
            PlayerPrefs.SetString(StringPlayerPrefs.BOOSTER_DATA, "");
            
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
            
        }


        public void Start()
        {
          
            SetData();
        }

        public void SetData()
        {
            for (int i = 0; i < boosterDatas.Boosters.Count; i++)
            {
                BoostersUI[i].SetData(this.boosterDatas.Boosters[i], i);
            }
        }

        public void ChangeAmountBooster(int indexSpecialSkill, int i)
        {
            boosterDatas.Boosters[indexSpecialSkill].Amount += i;
            //this.boosterDataS0.Boosters[indexSpecialSkill].Amount += i;
            SetData();
            SaveBoosterData();
            
            
        }

        public void SaveBoosterData()
        {
            String convert = JsonUtility.ToJson(boosterDatas);
            PlayerPrefs.SetString(StringPlayerPrefs.BOOSTER_DATA, convert);
        }
        public void OnApplicationQuit()
        {
          
           SaveBoosterData();
            
        }
    }
}