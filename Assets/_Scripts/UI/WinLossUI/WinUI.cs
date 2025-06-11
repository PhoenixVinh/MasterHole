using System.Collections;
using _Scripts.Event;
using _Scripts.Firebase;
using _Scripts.ManagerScene;
using _Scripts.ManagerScene.HomeScene;
using _Scripts.Sound;
using _Scripts.UI.AnimationUI;
using _Scripts.UI.PauseGameUI;
using _Scripts.UI.WinLossUI.SkinCollectionUI;
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
        public SkinProcess skinProcess;
        private int coinGet = 75;
        public Button homeBtn;


        public CollectionUI collection;

        public CoinRewardAnim coinRewardAnim;
        public override void OnEnable()
        {
            
            PlayerPrefs.SetInt(StringPlayerPrefs.LOSE_INDEX, 0);
            particle.Play();
            base.OnEnable();
            coinText.text = $"{coinGet}";
            int coin = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_COIN);
            coin += coinGet;
            text.text = $"{coin}";
            ManagerFirebase.Instance?.LogEarnResource(ResourceType.currency, ResourceName.Coin.ToString(),
                coinGet.ToString(), Reson.winlevel);
            coinRewardAnim.CountCoins(coin - coinGet, coin);
            PlayerPrefs.SetInt(StringPlayerPrefs.CURRENT_COIN, coin);
        
            

            StartCoroutine(DelayAppearButton());
            homeBtn.onClick.AddListener(ChangeHomeScene);
            int currentLevel = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_LEVEL);
          
            PlayerPrefs.SetInt(StringPlayerPrefs.CURRENT_LEVEL, currentLevel+1);
            collectionFeatureUI.SetData(currentLevel + 1);
        
            continueButton.onClick.AddListener(ShowNextlevel);
        }
        
        private void ChangeHomeScene()
        {
            SceneManager.LoadScene(EnumScene.HomeScene.ToString());
            ManagerFirebase.Instance?.ChangePositionFirebase(PositionFirebase.home);
            ManagerHomeScene.Instance.ShowRewardCoin(coinGet);
        }

        public void SetData(int coinGet)
        {
            this.coinGet = coinGet;
            this.gameObject.SetActive(true);
        }

       

        public override void OnDisable()
        {
            base.OnDisable();
            continueButton.onClick.RemoveAllListeners();
           
        }

        private void ShowNextlevel()
        {
            
            MaxAdsManager.Instance?.ShowInterAdsByLevel();
            
            int level = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_LEVEL);
            
            if (collection.CanShowContent(level))
            {
               
                this.gameObject.SetActive(false);
                collection.ShowContent(level);
            }
            
            else
            {
                this.gameObject.SetActive(false);
                // Change Data Level    
                if (skinProcess.GetTarget(level) != -1)
                {
                    skinProcess.gameObject.SetActive(true);
                }
                else
                {
                    ManagerLevelGamePlay.Instance.LoadNextLevel();
                }
                
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