using System;
using System.Threading.Tasks;
using _Scripts.Firebase;
using _Scripts.UI;
using UnityEngine;
using AppLovinMax;

public class MaxAdsManager : MonoBehaviour
{
    [SerializeField] private string SDK_KEY = "YOUR_SDK_KEY_HERE"; // Thay bằng SDK Key từ AppLovin Dashboard
    [SerializeField] private string bannerAdUnitId = "YOUR_BANNER_AD_UNIT_ID"; // ID quảng cáo biểu ngữ
    [SerializeField] private string interstitialAdUnitId = "YOUR_INTERSTITIAL_AD_UNIT_ID"; // ID quảng cáo xen kẽ
    [SerializeField] private string rewardedAdUnitId = "YOUR_REWARDED_AD_UNIT_ID"; // ID quảng cáo có tặng thưởng


    private bool isBannerLoaded = false;

    public static MaxAdsManager Instance;
    private bool isShowbanner = false;


    private DateTime timeLoadInter;
    private DateTime timeLoadRewarded;
    public bool isDoneBanner;

    public bool isRemoveInter = false;
    public bool isRemoveAds = false;


    private bool isInitMax = false;


    public float timeShowInter = 120;

    private DateTime _timeShowInter ;

    private DateTime _timeDoneReward;
    
    
    
    
    
    private int retryRewardAttempt = 0; // Số lần thử lại tải quảng cáo
    private int retryInterAttempt = 0; // Số lần thử lại tải quảng cáo xen kẽ
    private float maxRetryDelay = 64f; // Thời gian delay tối đa (giây)
    
    
    
    
    // timeBetween Rewarded vs Interstitial
    public float timeBetweenRewardedAndInterstitial = 15f; // Thời gian tối thiểu giữa quảng cáo có thưởng và quảng cáo xen kẽ
    private void Awake()
    {
        _timeShowInter = DateTime.Now;
        _timeDoneReward = DateTime.Now;
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }

     

        else
        {
            Destroy(this);
        }
        isRemoveInter = PlayerPrefs.GetInt(StringPlayerPrefs.REMOVED_ADS_PACK, 0) == 1;
        isRemoveAds = PlayerPrefs.GetInt(StringPlayerPrefs.REMOVED_ADS_VIP, 0) == 1;
    }


   

    public void InitAds()
    {
        if (isInitMax) return;
        
        MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration config) =>
        {
            Debug.Log("AppLovin SDK Initialized");
            // int IsTest = PlayerPrefs.GetInt(StringPlayerPrefs.ISTESTGAME, 0);
            // if(IsTest == 1)
            MaxSdk.SetCreativeDebuggerEnabled(true);
            MaxSdk.ShowMediationDebugger();
            //MaxSdk.ShowCreativeDebugger();
            InitializeBannerAds();
            InitializeInterstitialAds();
            InitializeRewardedAds();
        };

        MaxSdk.SetSdkKey(SDK_KEY);
        MaxSdk.InitializeSdk();
        isInitMax = true;
    }
    
    private void InitializeBannerAds()
    {
        // Tạo quảng cáo biểu ngữ
        MaxSdk.CreateBanner(bannerAdUnitId, MaxSdkBase.BannerPosition.BottomCenter);
        //MaxSdk.SetBannerBackgroundColor(bannerAdUnitId, Color.black);

        // Xử lý sự kiện tải quảng cáo
        MaxSdkCallbacks.Banner.OnAdLoadedEvent += (string adUnitId, MaxSdkBase.AdInfo adInfo) =>
        {
            Debug.Log("Banner Ad Loaded");
            isDoneBanner = true;
            ManagerFirebase.Instance?.LogIAA_AdShow(AdFormat.banner, AdPlatform.MaxApplovin.ToString(), adInfo.NetworkName, true, adInfo.Revenue, "USD");
        };
        MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += (string adUnitId, MaxSdkBase.ErrorInfo errorInfo) =>
        {
            Debug.Log("Banner Ad Failed to Load: " + errorInfo.Message);
            isBannerLoaded = false;
            ManagerFirebase.Instance?.LogIAA_AdShow(AdFormat.banner, AdPlatform.MaxApplovin.ToString(), "", true,0, "USD");
            // Thử tải lại sau một khoảng thời gian
            Invoke("LoadBannerAd", 5f);
        };
        
        
    }



    public async void ShowBannerAd()
    {
        if (isRemoveAds) return;
        
        isDoneBanner = false;
        MaxSdk.ShowBanner(bannerAdUnitId);
        Utills.DelayUntil(() =>
            isDoneBanner = true
        );
        
        isShowbanner = true;
    }

    public void HideBannerAd()
    {
        if (isShowbanner)
        {
            MaxSdk.HideBanner(bannerAdUnitId);
            isShowbanner = false;
        }
            
    }
    private void InitializeInterstitialAds()
    {
        // Tải quảng cáo xen kẽ
        MaxSdk.LoadInterstitial(interstitialAdUnitId);

        
        // Xử lý sự kiện
        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += (string adUnitId, MaxSdkBase.AdInfo adInfo) =>
        {
            
            ManagerFirebase.Instance?.LogIAA_AdRequest(AdFormat.interstitial, AdPlatform.MaxApplovin.ToString(), adInfo.NetworkName, true, Utills.GetMinusTime(this.timeLoadInter));
            retryInterAttempt = 0;
        };
        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += (string adUnitId, MaxSdkBase.ErrorInfo errorInfo) =>
        {
            ManagerFirebase.Instance?.LogIAA_AdRequest(AdFormat.interstitial, AdPlatform.MaxApplovin.ToString(), "", true, Utills.GetMinusTime(this.timeLoadInter));
            retryInterAttempt++;
            float retryDelay = Mathf.Pow(2, Mathf.Min(6, retryInterAttempt)); // Tăng delay, tối đa 64 giây
            Invoke("LoadInterstitialAd", retryDelay);
        };
        MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += (string adUnitId, MaxSdkBase.AdInfo adInfo) =>
        {
            ManagerFirebase.Instance?.LogIAA_AdShow(AdFormat.interstitial, AdPlatform.MaxApplovin.ToString(), adInfo.NetworkName,true, adInfo.Revenue, "USD");
        };
        
        
        MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += (string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo) =>
        {
            ManagerFirebase.Instance?.LogIAA_AdShow(AdFormat.interstitial, AdPlatform.MaxApplovin.ToString(), adInfo.NetworkName,false, 0, "USD");
        };

        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += (string adUnitId, MaxSdkBase.AdInfo adInfo) =>
        {
            ManagerFirebase.Instance?.LogIAA_ADComplete(AdFormat.interstitial, AdPlatform.MaxApplovin.ToString(), adInfo.NetworkName,EndType.quit.ToString(), Utills.GetMinusTime(this.timeLoadInter));   
            LoadInterstitialAd();
        };


    }
    
    
    
    
   
    private void InitializeRewardedAds()
    {
        // Tải quảng cáo có tặng thưởng
        MaxSdk.LoadRewardedAd(rewardedAdUnitId);

        // Xử lý sự kiện
        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += (string adUnitId, MaxSdkBase.AdInfo adInfo) =>
        {
            ManagerFirebase.Instance?.LogIAA_AdRequest(AdFormat.video_rewarded, AdPlatform.MaxApplovin.ToString(), adInfo.NetworkName, true, Utills.GetMinusTime(this.timeLoadRewarded));
            retryRewardAttempt = 0; // Reset số lần thử lại
        };
        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += (string adUnitId, MaxSdkBase.ErrorInfo errorInfo) =>
        {
            ManagerFirebase.Instance?.LogIAA_AdRequest(AdFormat.video_rewarded, AdPlatform.MaxApplovin.ToString(), "", true, Utills.GetMinusTime(this.timeLoadRewarded));
            retryRewardAttempt++;
            float retryDelay = Mathf.Pow(2, Mathf.Min(6, retryRewardAttempt)); // Tăng delay, tối đa 64 giây
            Invoke("LoadRewardedAd", retryDelay);
         
        };
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += (string adUnitId, MaxSdkBase.Reward reward, MaxSdkBase.AdInfo adInfo) =>
        {
            ManagerFirebase.Instance?.LogIAA_ADComplete(AdFormat.video_rewarded, AdPlatform.MaxApplovin.ToString(),
                adInfo.NetworkName, EndType.done.ToString(), Utills.GetMinusTime(this.timeLoadRewarded));
            
        };
        MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += (string adUnitId, MaxSdkBase.AdInfo adInfo) =>
        {
            ManagerFirebase.Instance?.LogIAA_AdShow(AdFormat.video_rewarded, AdPlatform.MaxApplovin.ToString(), adInfo.NetworkName,true, adInfo.Revenue, "USD");
        };
        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent +=
            (string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo) =>
            {
                ManagerFirebase.Instance?.LogIAA_AdShow(AdFormat.video_rewarded, AdPlatform.MaxApplovin.ToString(),
                    adInfo.NetworkName, false, 0, "USD");
            };
        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += (string adUnitId, MaxSdkBase.AdInfo adInfo) =>
        {
            LoadRewardedAd();
            ManagerFirebase.Instance?.LogIAA_ADComplete(AdFormat.video_rewarded, AdPlatform.MaxApplovin.ToString(),
                adInfo.NetworkName, EndType.quit.ToString(), Utills.GetMinusTime(this.timeLoadRewarded));
            _timeDoneReward = DateTime.Now; // Reset thời gian chờ sau khi quảng cáo hoàn thành
            
        };
    }

    public void ShowRewardedAd(Action callback = null)
    {
       
        timeLoadRewarded = DateTime.Now;
        if (MaxSdk.IsRewardedAdReady(rewardedAdUnitId))
        {
            // Create a local handler
            Action<string, MaxSdkBase.Reward, MaxSdkBase.AdInfo> handler = null;
            handler = (string adUnitId, MaxSdkBase.Reward reward, MaxSdkBase.AdInfo adInfo) =>
            {
                callback?.Invoke();
                LoadRewardedAd();
                // Unsubscribe after called
                MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent -= handler;
            };
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += handler;

            MaxSdk.ShowRewardedAd(rewardedAdUnitId);
        }
        else
        {
            LoadRewardedAd();
            Debug.Log("Rewarded ad is not ready");
        }
    }
    

    
    // // Hàm hiển thị Banner Ad
    // public void ShowBannerAd()
    // {
    //     if (isBannerLoaded)
    //     {
    //         MaxSdk.ShowBanner(bannerAdUnitId);
    //         Debug.Log("Banner Ad Shown");
    //     }
    //     else
    //     {
    //         Debug.Log("Banner Ad is not loaded yet, retrying...");
    //         LoadBannerAd(); // Thử tải lại nếu chưa sẵn sàng
    //     }
    // }
    // Hàm ẩn Banner Ad
  

    // Hàm tải lại Banner Ad
    private void LoadBannerAd()
    {
        MaxSdk.CreateBanner(bannerAdUnitId, MaxSdkBase.BannerPosition.BottomCenter);
    }
    
    private void LoadInterstitialAd()
    {
        MaxSdk.LoadInterstitial(interstitialAdUnitId);
        timeLoadInter = DateTime.Now;
    }
    private void LoadRewardedAd()
    {
        MaxSdk.LoadRewardedAd(rewardedAdUnitId);
        timeLoadRewarded = DateTime.Now;
    }




    public void ShowInterAdsByLevel(Action callback = null)
    {
        
        
        if(isRemoveAds || isRemoveInter) return;
        
        int currentLevel = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_LEVEL);
        
        
        
        Debug.Log(ManagerFirebase.Instance.firebaseInitial.Level_Show_Inter + "???" + ManagerFirebase.Instance.firebaseInitial.Inter_Distance_Level);

        if (Utills.GetMinusTime(_timeDoneReward) / 1000f < timeBetweenRewardedAndInterstitial) return;
        
            
        
        float value = Utills.GetMinusTime(_timeShowInter)/1000f;
        if (value >= timeShowInter )
        {
            MaxSdk.ShowInterstitial(interstitialAdUnitId);
            LoadInterstitialAd();
            _timeShowInter = DateTime.Now;
            
            return;
            
        }
        // if (currentLevel >= ManagerFirebase.Instance.firebaseInitial.Level_Show_Inter &&
        //     (currentLevel - ManagerFirebase.Instance.firebaseInitial.Level_Show_Inter) %
        //     ManagerFirebase.Instance.firebaseInitial.Inter_Distance_Level == 0)
        // {
        //     MaxSdk.ShowInterstitial(interstitialAdUnitId);
        //     LoadInterstitialAd();
        // }
       
    }
}