using System.Collections;
using _Scripts.ManagerScene;
using _Scripts.ManagerScene.HomeScene;
using _Scripts.Sound;
using _Scripts.UI.PauseGameUI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;


namespace _Scripts.UI.WinLossUI
{
    public class WinUI : PauseGame
    {
        public TMP_Text coinText;
        public Button continueButton;
        
        
        public ParticleSystem particle;

        private int coinGet = 75;
        public Button homeBtn;


        public CollectionUI collection;
        public override void OnEnable()
        {
           
            
            particle.Play();
            base.OnEnable();
            coinText.text = $"{coinGet}";
            int coin = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_COIN);
            coin += 75;
            PlayerPrefs.SetInt(StringPlayerPrefs.CURRENT_COIN, coin);
            continueButton.onClick.AddListener(ShowNextlevel);
           
            StartCoroutine(DelayAppearButton());
            homeBtn.onClick.AddListener(ChangeHomeScene);
        }

        private void ChangeHomeScene()
        {
            SceneManager.LoadScene(EnumScene.HomeScene.ToString());
            ManagerHomeScene.Instance.ShowRewardCoin(coinGet);
        }

        public void SetData(int coinGet)
        {
            this.coinGet = coinGet;
        }

       

        public override void OnDisable()
        {
            base.OnDisable();
            continueButton.onClick.RemoveAllListeners();
           
        }

        private void ShowNextlevel()
        {
            int level = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_LEVEL);
            if (collection.CanShowContent(level))
            {
                collection.ShowContent();
                this.gameObject.SetActive(false);
            }
            else
            {
                this.gameObject.SetActive(false);
                // Change Data Level 

                ManagerLevelGamePlay.Instance.LoadNextLevel();
            }
            
           
           
        }
        private IEnumerator DelayAppearButton()
        {
            homeBtn.gameObject.SetActive(false);
            yield return new WaitForSecondsRealtime (3f);
            homeBtn.gameObject.SetActive(true);
        }

    }
}