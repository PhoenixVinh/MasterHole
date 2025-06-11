using System;
using System.Collections;
using System.Threading.Tasks;
using _Scripts.Firebase;
using _Scripts.ManagerScene;
using _Scripts.ManagerScene.HomeScene;
using _Scripts.ObjectPooling;
using _Scripts.UI.PauseGameUI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Scripts.UI.WinLossUI
{
    public class Lose2UI: PauseGame
    {
        
   
        public Button homeBtn;
        public Button playAgainBtn;

        
        public  override void OnEnable()
        {
            
            base.OnEnable();
            StartCoroutine(DelayAppearButton());
            playAgainBtn.onClick.AddListener(PlayAgain);
            ManagerFirebase.Instance?.LogSpendResource(ResourceType.item, ResourceName.Heart.ToString(), "1", Reson.use);
            homeBtn.onClick.AddListener(goHome);
            DateTime inifity = Utills.StringToDate(PlayerPrefs.GetString(StringPlayerPrefs.UNLIMITED_TIME));
            if (inifity <= DateTime.Now)
            {
                if(!PlayerPrefs.HasKey(StringPlayerPrefs.CURRENT_ENERGY)){
                    PlayerPrefs.SetInt(StringPlayerPrefs.CURRENT_ENERGY, 5);
                }
                int currentEnergy = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_ENERGY);
                currentEnergy--;
                PlayerPrefs.SetInt(StringPlayerPrefs.CURRENT_ENERGY, currentEnergy);
                PlayerPrefs.Save();
                
            }
            
        }
        public override void OnDisable()
        {
            base.OnDisable();
            homeBtn.onClick.RemoveAllListeners();
            playAgainBtn.onClick.RemoveAllListeners();
        }
        
        
        private async void PlayAgain()
        {
            
            DateTime inifity = Utills.StringToDate(PlayerPrefs.GetString(StringPlayerPrefs.UNLIMITED_TIME));
            if (inifity <= DateTime.Now)
            {
                int currentEnergy = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_ENERGY);
                if (currentEnergy != 0)
                {
                    this.gameObject.SetActive(false);
                    ManagerLevelGamePlay.Instance.LoadLevelAgain();
                }
                else
                {
                    MessagePooling.Instance.SpawnMessage(this.playAgainBtn.transform.position, Utills.NOT_ENOUGH_ENERGY);
                }
                
            }
            else
            {
                this.gameObject.SetActive(false);
                ManagerLevelGamePlay.Instance.LoadLevelAgain();
            }
            
        }

        private void goHome()
        {
            this.gameObject.SetActive(false);
            MaxAdsManager.Instance?.ShowInterAdsByLevel();
           
            //ManagerHomeScene.Instance?.MinusHealth();
            SceneManager.LoadScene(EnumScene.HomeScene.ToString());
            ManagerFirebase.Instance?.ChangePositionFirebase(PositionFirebase.home);
            
            
        }

        private IEnumerator DelayAppearButton()
        {
            homeBtn.gameObject.SetActive(false);
            yield return new WaitForSecondsRealtime(3f);
            homeBtn.gameObject.SetActive(true);
        }
    }
}