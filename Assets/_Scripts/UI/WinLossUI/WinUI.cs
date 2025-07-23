
using System;
using System.Collections;
using _Scripts.Firebase;
using _Scripts.ManagerScene;
using _Scripts.ManagerScene.HomeScene;
using _Scripts.Sound;
using _Scripts.UI.AnimationUI;
using _Scripts.UI.PauseGameUI;
using _Scripts.UI.WinLossUI.SkinCollectionUI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;


namespace _Scripts.UI.WinLossUI
{
    public class WinUI : PauseGame
    {
        [SerializeField] private TMP_Text coinResouce;
        
        
        public TMP_Text coinText;
        public TMP_Text coinTextBtn;
        public Button RewardBtn;
        
        
        public ParticleSystem particle;
        //public CollectionFeatureUI collectionFeatureUI;
        public SkinProcess skinProcess;
        [SerializeField]private int coinDefault = 100;
        public Button continueBtn;


        //public CollectionUI collection;

        public CoinRewardAnim coinRewardAnim;
        
        [Header("Spinner")]
        [SerializeField] private Spinner spinner;
        [SerializeField] private GameObject rewardSpinnerGO;

        private int coinRewardGet = 100;



        private bool isClickBtn = false;

        public override void OnEnable()
        {
            // Set active GO
            //rewardSpinnerGO.SetActive(true);
            RewardBtn.gameObject.SetActive(true);
            continueBtn.gameObject.SetActive(true);
            isClickBtn = false;
            
            
            
            coinResouce.text = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_COIN, 900).ToString();            
            
            ManagerSound.Instance?.PlayEffectSound(EnumEffectSound.Victory);
            PlayerPrefs.SetInt(StringPlayerPrefs.LOSE_INDEX, 0);
            particle.Play();
            //spinner.StartParabola();
            base.OnEnable();
            
            
            //  Using anim right here 
            // coinText.text = $"{coinGet}";
            
        
            

            //StartCoroutine(DelayAppearButton());
          


            if (!PlayerPrefs.HasKey(StringPlayerPrefs.CURRENT_LEVEL))
            {
                PlayerPrefs.SetInt(StringPlayerPrefs.CURRENT_LEVEL, 1);
            }
            int currentLevel = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_LEVEL);
          
            PlayerPrefs.SetInt(StringPlayerPrefs.CURRENT_LEVEL, currentLevel+1);
            //collectionFeatureUI.SetData(currentLevel + 1);
            RewardBtn.onClick.AddListener(OnRewardClick);
            continueBtn.onClick.AddListener(OnNoRewardClick);
           
        }


        public void Update()
        {
            // Set display Coin follow by spinner 
            if(isClickBtn) return;
            coinRewardGet = (int)(coinDefault * GetMultiplyCoin(spinner.GetIndexPosition()));
            
            coinText.text = $"{coinRewardGet}";
            coinTextBtn.text = $"{coinRewardGet}";
            
            
            
            
            
        }


        private void ChangeHomeScene()
        {
            //MaxAdsManager.Instance?.ShowInterAdsByLevel();
            SceneManager.LoadScene(EnumScene.HomeScene.ToString());
            ManagerFirebase.Instance?.ChangePositionFirebase(PositionFirebase.home);
            ManagerHomeScene.Instance.ShowRewardCoin(coinDefault);
        }

        public void SetData(int coinGet)
        {
            this.coinDefault = coinGet;
         
        }
    
       

        public override void OnDisable()
        {
            base.OnDisable();
            RewardBtn.onClick.RemoveAllListeners();
            continueBtn.onClick.RemoveAllListeners();
            StopAllCoroutines();
           
        }

        private  void ShowNextlevel()
        {
            if(ManagerFirebase.Instance != null)
                if(ManagerFirebase.Instance.firebaseInitial.trigger_inter_continue_win)
                    MaxAdsManager.Instance?.ShowInterAdsByLevel();
            
            int level = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_LEVEL);
            
            this.gameObject.SetActive(false);
            ManagerLevelGamePlay.Instance.LoadNextLevel();
            // if (collection.CanShowContent(level))
            // {
            //    
            //     this.gameObject.SetActive(false);
            //     collection.ShowContent(level);
            // }
            //
            // else
            // {
            //     //this.gameObject.SetActive(false);
            //     // Change Data Level    
            //     if (skinProcess.GetTarget(level) != -1)
            //     {
            //         this.gameObject.SetActive(false);
            //         skinProcess.gameObject.SetActive(true);
            //     }
            //     else
            //     {
            //         
            //     }
            //     
            // }
            
           
           
        }
        private IEnumerator DelayAppearButton()
        {
            continueBtn.gameObject.SetActive(false);
            yield return new WaitForSecondsRealtime (3f);
            continueBtn.gameObject.SetActive(true);
        }

        private float GetMultiplyCoin(int index)
        {
            if (index == 2) return 3f;
            if (index == 1 || index == 3) return 2f;
            return 1.5f;
           
        }

        private void OnRewardClick()
        {
            spinner.StopSpinner();
            MaxAdsManager.Instance?.ShowRewardedAd(() =>
            {
                
                
                // Adding coin using anim
                
                ManagerFirebase.Instance?.LogEarnResource(ResourceType.currency, ResourceName.Coin.ToString(),
                    coinRewardGet.ToString(), Reson.reward);
                AddCoin(this.coinRewardGet, true);
                
            });
            
            
           
        }

        private void OnNoRewardClick()
        {
            spinner.StopSpinner();
            AddCoin(this.coinDefault, false);
        }


        public void AddCoin(int amount, bool isWatchReward)
        {
            isClickBtn = true;
            //this.rewardSpinnerGO.SetActive(false);
            RewardBtn.gameObject.SetActive(false);
            continueBtn.gameObject.SetActive(false);
            if (!isWatchReward)
            {
                ManagerFirebase.Instance?.LogEarnResource(ResourceType.currency, ResourceName.Coin.ToString(),
                    amount.ToString(), Reson.winlevel);
            }
            
            
          
            
           
            int coin = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_COIN,900);
            coin += amount; 
            PlayerPrefs.SetInt(StringPlayerPrefs.CURRENT_COIN, coin);
            
     
            
            StartCoroutine(DelayAnimCourutine(coin - amount, coin, isWatchReward));
        }

        private IEnumerator DelayAnimCourutine(int startCoin, int endCoin, bool isReward)
        {

            if (isReward)
            {
                yield return new WaitUntil(() => MaxAdsManager.Instance.isHiddenReward);
            }
            this.coinText.text = $"{endCoin - startCoin}";
            
            //if(ManagerFirebase.Instance)
            
            coinRewardAnim.CountCoins(startCoin, endCoin);
            yield return new WaitUntil( () => this.coinRewardAnim.isAnim == false);
            //yield return new WaitForSecondsRealtime(0.1f);
            ShowNextlevel();

        }
    }
}