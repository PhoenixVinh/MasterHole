using System;
using System.Collections;
using _Scripts.ManagerScene;
using _Scripts.ManagerScene.HomeScene;
using _Scripts.ObjectPooling;
using _Scripts.Sound;
using _Scripts.UI.PauseGameUI;

using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.UI;


namespace _Scripts.UI.WinLossUI
{
    public class LossUI: PauseGame
    {
        [Header("Group Button")]
        public Button playOnBtn;
        public Button retryBtn;
        public Button continueBtn;

        
        public float timeAdd = 60f;
        public int pricePlayOn = 600;

        [Header("Lose UI 2")] public GameObject loseUI2;
        
        public override void OnEnable()
        {
            base.OnEnable();
            playOnBtn.onClick.AddListener(OnClickPlayOnBtn);
            retryBtn.onClick.AddListener(OnClickRetryBtn);
            continueBtn.onClick.AddListener(OnClickContinueBtn);
            this.loseUI2.SetActive(false);
            StartCoroutine(DelayAppearButton());
            
            
            int currentLoseIndex = PlayerPrefs.GetInt(StringPlayerPrefs.LOSE_INDEX);
            currentLoseIndex++;
            PlayerPrefs.SetInt(StringPlayerPrefs.LOSE_INDEX, currentLoseIndex);
            
            
            DateTime inifity = Utills.StringToDate(PlayerPrefs.GetString(StringPlayerPrefs.UNLIMITED_TIME));
            if (inifity < DateTime.Now)
            {
                int currentEnergy = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_ENERGY);
                currentEnergy--;
                PlayerPrefs.SetInt(StringPlayerPrefs.CURRENT_ENERGY, currentEnergy);
            }
            
            

        }

        private IEnumerator DelayAppearButton()
        {
            continueBtn.gameObject.SetActive(false);
            yield return new WaitForSecondsRealtime (3f);
            continueBtn.gameObject.SetActive(true);
        }
        


        public override void OnDisable()
        {
            base.OnDisable();
            playOnBtn.onClick.RemoveAllListeners();
            retryBtn.onClick.RemoveAllListeners();
            continueBtn.onClick.RemoveAllListeners();
            
        }

        private void OnClickContinueBtn()
        {
            this.gameObject.SetActive(false);
            this.loseUI2.SetActive(true);
            
        }

        private void OnClickRetryBtn()
        {
           
            AddTimeGamePlay();
            this.gameObject.SetActive(false);   
            
        }

        private void OnClickPlayOnBtn()
        {
            // Check if have enough gold then minus it and Add Time to the Game play 
            
           
            int coin = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_COIN);
            if (coin >= pricePlayOn)
            {
                AddTimeGamePlay();
                coin -= pricePlayOn;
                PlayerPrefs.SetInt(StringPlayerPrefs.CURRENT_COIN, coin);
                this.gameObject.SetActive(false);   
            }
            else
            {
                MessagePooling.Instance.SpawnMessage(this.playOnBtn.transform.position);
            }
            
            
            
        }
        
        


        public void AddTimeGamePlay()
        {
            ColdownTime.Instance.AddTime(timeAdd);
        }
    }
}