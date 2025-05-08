using System;
using System.Collections.Generic;
using _Scripts.UI.BoosterUI;
using Unity.VisualScripting;
using UnityEngine;

namespace _Scripts.Booster
{
    public class ManagerBooster : MonoBehaviour
    {
        
        public static ManagerBooster Instance;
        public BoosterDataSO boosterDataS0;
        List<BoosterData> boostersData = new List<BoosterData>();

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

            if (boostersData == null)
            {
                boosterDataS0 = Resources.Load<BoosterDataSO>("BoosterSO/BoosterData");
            }
            
        }


        public void Start()
        {
            boostersData = boosterDataS0.Boosters;
            SetData();
        }

        public void SetData()
        {
            for (int i = 0; i < boostersData.Count; i++)
            {
                BoostersUI[i].SetData(this.boostersData[i], i);
            }
        }

        public void ChangeAmountBooster(int indexSpecialSkill, int i)
        {
            this.boostersData[indexSpecialSkill].Amount += i;
            //this.boosterDataS0.Boosters[indexSpecialSkill].Amount += i;
            SetData();
        }
    }
}