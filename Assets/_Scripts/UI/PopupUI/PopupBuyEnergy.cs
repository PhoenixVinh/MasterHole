using System;
using System.Collections.Generic;
using _Scripts.Event;
using _Scripts.ObjectPooling;
using _Scripts.UI.HomeSceneUI.ResourcesUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI.PopupUI
{
    public class PopupBuyEnergy : MonoBehaviour
    {

        [SerializeField]private GameObject message;
        
        public List<ItemEnergyPopup> energyObjects;

        private int maxEnergy;
        private int currentEnergy;



        public Button refillBtn;
        public Button adsBtn;

        public int price;
        
        private bool canBuy = false;
        private void Start()
        {
            maxEnergy = Energy.Instance.MaxEnergy;
            
        }

        private void OnEnable()
        {
            refillBtn.onClick.AddListener(BuyEnergyByCoin);
            adsBtn.onClick.AddListener(BuyEnergyByAds);
        }

        private void OnDisable()
        {
            refillBtn.onClick.RemoveAllListeners();
            adsBtn.onClick.RemoveAllListeners();
        }

        private void BuyEnergyByAds()
        {
            if(!canBuy) return;
            
            MaxAdsManager.Instance.ShowRewardedAd(() =>
            {
                ResourceEvent.OnUpdateResource?.Invoke();
                Resource.Instance?.PlusHealth();
                    
                UpdateUI();
            });
         
                
            
        }

        private void BuyEnergyByCoin()
        {
            if(!canBuy) return;
            if (Resource.Instance.useCoin(price))
            {
                ResourceEvent.OnUpdateResource?.Invoke();
                Resource.Instance?.PlusHealth();
                    
                UpdateUI();
            }
            else
            {
                GameObject messageuUI =  Instantiate(message, refillBtn.transform.position, Quaternion.identity);
                messageuUI.GetComponent<TMP_Text>().text = Utills.NOT_ENOUGH_COIN;
                messageuUI.transform.SetParent(transform);
                
            }
            
        }

        private void UpdateUI()
        {
            currentEnergy = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_ENERGY);
            if (currentEnergy == maxEnergy)
            {
                refillBtn.image.color = new Color(0.5f, 0.5f, 0.5f, 1f);
                adsBtn.image.color = new Color(0.5f, 0.5f, 0.5f, 1f);
                canBuy = false;
                for (int i = 1; i <= maxEnergy; i++)
                {
                    energyObjects[i-1].SetData(true,"");
                }
            }
            else
            {
                canBuy = true;
                
                refillBtn.image.color = new Color(1f, 1f, 1f, 1f);
                adsBtn.image.color = new Color(1f, 1f, 1f, 1f);
                for (int i = 1; i <= currentEnergy; i++)
                {
                    energyObjects[i-1].SetData(true,"");
                }
                energyObjects[currentEnergy].SetData(false, Energy.Instance.TimeValue);

                if (currentEnergy + 2 > maxEnergy) return;
                for (int i = currentEnergy+1; i < maxEnergy; i++)
                {
                    energyObjects[i].SetData(false, "");
                }
            }
        }


        private void FixedUpdate()
        {
            UpdateUI();

        }

        
    }
}