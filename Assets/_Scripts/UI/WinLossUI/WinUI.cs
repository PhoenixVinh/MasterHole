using System.Collections;
using _Scripts.Event;
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
        [SerializeField] private TMP_Text text;
        
        
        public TMP_Text coinText;
        public Button continueButton;
        
        
        public ParticleSystem particle;
        public CollectionFeatureUI collectionFeatureUI;
        private int coinGet = 75;
        public Button homeBtn;


        public CollectionUI collection;
        public override void OnEnable()
        {
           
            
            particle.Play();
            base.OnEnable();
            coinText.text = $"{coinGet}";
            int coin = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_COIN);
            coin += coinGet;
            text.text = $"{coin}";
            RewardCoinEvent.OnRewardCoin?.Invoke(coin - coinGet, coin);
            PlayerPrefs.SetInt(StringPlayerPrefs.CURRENT_COIN, coin);
            continueButton.onClick.AddListener(ShowNextlevel);
           
            StartCoroutine(DelayAppearButton());
            homeBtn.onClick.AddListener(ChangeHomeScene);
            collectionFeatureUI.SetData(PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_LEVEL,1) + 1);
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
                collection.ShowContent(level);
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