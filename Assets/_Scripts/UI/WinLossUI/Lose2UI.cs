using System;
using System.Collections;
using _Scripts.Firebase;
using _Scripts.ManagerScene;
using _Scripts.UI.PauseGameUI;
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

        
        public override void OnEnable()
        {
            
            base.OnEnable();
            StartCoroutine(DelayAppearButton());
            playAgainBtn.onClick.AddListener(PlayAgain);
           
            homeBtn.onClick.AddListener(goHome);
            
        }
        public override void OnDisable()
        {
            base.OnDisable();
            // homeBtn.onClick.RemoveAllListeners();
            // playAgainBtn.onClick.RemoveAllListeners();
        }
        
        
        private void PlayAgain()
        {
            this.gameObject.SetActive(false);
            
            ManagerLevelGamePlay.Instance.LoadLevelAgain();
        }

        private void goHome()
        {
            
            MaxAdsManager.Instance?.ShowInterAdsByLevel();
            this.gameObject.SetActive(false);
            
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