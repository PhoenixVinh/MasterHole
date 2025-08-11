using System;
using System.Collections;
using System.Diagnostics;
using _Scripts.Booster;
using _Scripts.ManagerScene;
using _Scripts.UI;
using _Scripts.UI.MissionUI;
using Firebase.Analytics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

namespace _Scripts.Firebase
{
    public class ManagerFirebase : MonoBehaviour
    {
        [SerializeField]public FirebaseInitial firebaseInitial;
        public static ManagerFirebase Instance;
        
        public PositionFirebase positionFirebase;
        public PositionFirebase positionPopup;
        
        
        
        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(this);
            }
        }

        private void Start()
        {
            StartCoroutine(LogDataLevelEnd());
            positionFirebase = PositionFirebase.home;
            positionPopup = PositionFirebase.none;
        }


        public void ChangePositionFirebase(PositionFirebase position)
        {
            this.positionFirebase = position;
        }
        

        private IEnumerator LogDataLevelEnd()
        {
            if (!PlayerPrefs.HasKey(StringPlayerPrefs.IS_LOG_EVENT_PLAYER_END))
            {
                PlayerPrefs.SetInt(StringPlayerPrefs.IS_LOG_EVENT_PLAYER_END, 0);
            }
            int Islog = PlayerPrefs.GetInt(StringPlayerPrefs.IS_LOG_EVENT_PLAYER_END);
            if (Islog == 0)
            {
                
            }
            else if (Islog == 1 && GetNetworkStatus() != InternetStatus.offline)
            {
                
                
                yield return new WaitUntil(() => firebaseInitial.firebaseInitialized);
                LevelEndData levelEndData = JsonUtility.FromJson<LevelEndData>(PlayerPrefs.GetString(StringPlayerPrefs.LEVEL_END_PLAYER));
                LogFirebaseDataLevelEnd(levelEndData);
                PlayerPrefs.SetInt(StringPlayerPrefs.IS_LOG_EVENT_PLAYER_END, 0);

            }
        }


        public void LogEventWithString(string eventName)
        {
            if (firebaseInitial.firebaseInitialized)
                FirebaseAnalytics.LogEvent(eventName);
        }
        
       

        public void LogLevelStart(int playerLevel, PlayType playType, int msec, int play_index, int lose_index )
        {
            if (!firebaseInitial.firebaseInitialized)
            {
                Debug.Log("Log Event Fail");
                return;
            }
            Debug.Log("Log Event Success");
            FirebaseAnalytics.LogEvent(
                EnumValueFirebase.level_start.ToString(),
                new Parameter("level", playerLevel),
                new Parameter("play_type", playType.ToString()),
                new Parameter("total_duration_start", msec),
                new Parameter("play_index", play_index),
                new Parameter("lose_index", lose_index)
                
            );
        }
        
        
        public void LogLevelEnd(LevelResult levelResult, LoseBy loseBy )
        {
            if (!firebaseInitial.firebaseInitialized)
            {
                Debug.Log("Log Event Fail");
                return;
            }
            string sceneName = SceneManager.GetActiveScene().name;
            if (sceneName != EnumScene.PlayScene.ToString()) return;
            LevelEndData levelEndData = GetLevelEndData(levelResult.ToString(), loseBy.ToString());
            LogFirebaseDataLevelEnd(levelEndData);
        }


        private void LogFirebaseDataLevelEnd( LevelEndData levelEndData)
        {
            FirebaseAnalytics.LogEvent(
                EnumValueFirebase.level_end.ToString(),
                new Parameter("level", levelEndData.PlayerLevel),
                new Parameter("play_type", levelEndData.PlayerType),
                new Parameter("total_duration_start", levelEndData.TimeStart),
                new Parameter("total_duration_end", levelEndData.TimeEnd),
                new Parameter("remain_duration",levelEndData.RemainDuration),
                new Parameter("time_duration", levelEndData.TimeDuration),
                new Parameter("play_index", levelEndData.PlayIndex),
                new Parameter("lose_index", levelEndData.LoseIndex),
                new Parameter("total_food", levelEndData.TotalFood),
                new Parameter("cleared_food", levelEndData.ClearedFood),
                new Parameter("level_result", levelEndData.LevelResult),
                new Parameter("lose_by", levelEndData.LoseBy)
                
            );
            
            Debug.Log("Log Event Success");
        }


        public void LogEarnResource(ResourceType resourceType, string resourceName, string resouceAmount, Reson reson)
        {
            if (!firebaseInitial.firebaseInitialized)
            {
                Debug.Log("Log Event Fail");
                return;
            }
            Debug.Log("Log Event Success");

            string pos = positionPopup != PositionFirebase.none ? positionPopup.ToString() : positionFirebase.ToString();
            
            
            FirebaseAnalytics.LogEvent(
                EnumValueFirebase.earn_resources.ToString(),
                new Parameter("resource_type", resourceType.ToString()),
                new Parameter("resource_name", resourceName),
                new Parameter("resource_amount", resouceAmount),
                new Parameter("reason", reson.ToString()),
                new Parameter("position", pos)
                
            );
        }

        public void LogSpendResource(ResourceType resourceType, string resourceName, string resourceAmount, Reson reson)
        {
            if (!firebaseInitial.firebaseInitialized)
            {
                Debug.Log("Log Event Fail");
                return;
            }
            Debug.Log("Log Event Success");

            string pos = positionPopup != PositionFirebase.none ? positionPopup.ToString() : positionFirebase.ToString();
            
            
            FirebaseAnalytics.LogEvent(
                EnumValueFirebase.spend_resources.ToString(),
                new Parameter("resource_type", resourceType.ToString()),
                new Parameter("resource_name", resourceName),
                new Parameter("resource_amount", resourceAmount),
                new Parameter("reason", reson.ToString()),
                new Parameter("position", pos)
                
            );
        }

        public void LogIAA_AdRequest(AdFormat adformat, string ad_platform,string ad_network, bool isLoad, float time)
        {
            if (!firebaseInitial.firebaseInitialized)
            {
                Debug.Log("Log Event Fail");
                return;
            }
            Debug.Log("Log Event Success");


            string placement = "";
            
           
            switch (adformat)
            {
                case AdFormat.interstitial:
                    placement = GetPlacement(StringPlayerPrefs.FIRST_OPEN_LEVEL_INTER);
                    break;
                case AdFormat.video_rewarded:
                    placement = GetPlacement(StringPlayerPrefs.FIRST_OPEN_LEVEL_REWARDS);
                    break;
            }
            FirebaseAnalytics.LogEvent(
                EnumValueFirebase.ad_request.ToString(),
                new Parameter("ad_format", adformat.ToString()),
                new Parameter("ad_platform", ad_platform),
                new Parameter("ad_network", ad_network),
                new Parameter("placement", placement),
                new Parameter("is_show", isLoad ? "0" : "1"),
                new Parameter("time", time)
                
            );
            
        }

        public void LogIAA_AdShow(AdFormat adformat, string ad_platform, string ad_network, bool isShow, double value,
            string currency)
        {
            if (!firebaseInitial.firebaseInitialized)
            {
                Debug.Log("Log Event Fail");
                return;
            }
            Debug.Log("Log Event Success");
            string pos = positionPopup != PositionFirebase.none ? positionPopup.ToString() : positionFirebase.ToString();
            
            
            FirebaseAnalytics.LogEvent(
                EnumValueFirebase.ad_show.ToString(),
                new Parameter("ad_format", adformat.ToString()),
                new Parameter("ad_platform", ad_platform),
                new Parameter("ad_network", ad_network),
                new Parameter("placement", pos),
                new Parameter("is_show", isShow ? 0 : 1),
                new Parameter("value", value),
                new Parameter("currency", currency)
                
                
            );

            
        }
        

        public void LogIAA_ADComplete(AdFormat adformat, string ad_platform, string ad_network, string end_type,
            double ad_duration)
        {
            if (!firebaseInitial.firebaseInitialized)
            {
                Debug.Log("Log Event Fail");
                return;
            }
      
            string pos = positionPopup != PositionFirebase.none ? positionPopup.ToString() : positionFirebase.ToString();
            
            
            FirebaseAnalytics.LogEvent(
                EnumValueFirebase.ad_complete.ToString(),
                new Parameter("ad_format", adformat.ToString()),
                new Parameter("ad_platform", ad_platform),
                new Parameter("ad_network", ad_network),
                new Parameter("end_type", end_type),
                new Parameter("ad_duration", ad_duration),
                new Parameter("placement", pos)
              
                
                
            );

        }


        public void LogIAP_Click(ShowType showtype, string packname)
        {
            if (!firebaseInitial.firebaseInitialized)
            {
                Debug.Log("Log Event Fail");
                return;
            }
            Debug.Log("Log Event Success");
            string pos = positionPopup != PositionFirebase.none ? positionPopup.ToString() : positionFirebase.ToString();
            
            
            FirebaseAnalytics.LogEvent(
                EnumValueFirebase.iap_click.ToString(),
                new Parameter("placement", pos),
                new Parameter("show_type", showtype.ToString()),
                new Parameter("pack_name", packname)
                
              
              
                
                
            );
        }

        public void LogIAP_Purchase(ShowType showtype,string packname, double price, string currency)
        {
            if (!firebaseInitial.firebaseInitialized)
            {
                Debug.Log("Log Event Fail");
                return;
            }
            Debug.Log("Log Event Success");
            string pos = positionPopup != PositionFirebase.none ? positionPopup.ToString() : positionFirebase.ToString();
            
            
            FirebaseAnalytics.LogEvent(
                EnumValueFirebase.iap_purchase.ToString(),
                new Parameter("placement", pos),
                new Parameter("show_type", showtype.ToString()),
                new Parameter("pack_name", packname),
                new Parameter("price", price),
                new Parameter("currency", currency)
              
              
                
                
            );
        }

        public string GetPlacement(string key)
        {
            string result = "";
            if (!PlayerPrefs.HasKey(StringPlayerPrefs.LEVEL_SHOW_BANNER))
            {
                result = "open_game";
            }
            else
            {
                PlayerPrefs.SetString(StringPlayerPrefs.LEVEL_SHOW_BANNER, "Yes");
                result = "auto_reload";
            }
            return result;
         
        }
        

        private void OnApplicationPause(bool pauseStatus)
        {
           
            string sceneName = SceneManager.GetActiveScene().name;
            if (sceneName != EnumScene.PlayScene.ToString()) return;
            
            if (pauseStatus)
            {
                string jsonData =
                    JsonUtility.ToJson(GetLevelEndData(LevelResult.exitgame.ToString(), LoseBy.Null.ToString()));
               PlayerPrefs.SetString(StringPlayerPrefs.LEVEL_END_PLAYER, jsonData);
               PlayerPrefs.SetInt(StringPlayerPrefs.IS_LOG_EVENT_PLAYER_END, 1);
               
            }
            else
            {
                PlayerPrefs.SetInt(StringPlayerPrefs.IS_LOG_EVENT_PLAYER_END, 0);
            }
        }

        private LevelEndData GetLevelEndData(string LevelResult, string LoseBy)
        {
            
            int getCurrentBoosterTime = PlayerPrefs.GetInt(StringPlayerPrefs.COUNT_USE_BOOSTER_ICE);
            LevelEndData levelEndData = new LevelEndData
            {
                PlayerLevel = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_LEVEL),
                PlayerType = PlayerPrefs.GetString(StringPlayerPrefs.PLAYER_TYPE),
                TimeStart = (int)ManagerLevelGamePlay.Instance?.level.timeToComplete*1000,
                TimeEnd =  (int)ManagerLevelGamePlay.Instance?.level.timeToComplete*1000 + getCurrentBoosterTime*12*1000,
                RemainDuration = (int)(ColdownTime.Instance.RemainTime*1000),
                PlayIndex =  PlayerPrefs.GetInt(StringPlayerPrefs.LOSE_INDEX),
                LoseIndex = PlayerPrefs.GetInt(StringPlayerPrefs.LOSE_INDEX),
                TotalFood = ManagerMission.Instance.AmountMissionItem,
                ClearedFood = ManagerMission.Instance.GetClearFood(),
                LevelResult = LevelResult,
                LoseBy =  LoseBy
                

            };
            return levelEndData;
        }

        
        
        
        
        
        
        
        
        
        
        

        public void SetPlayerProperties()
        {
            if (!firebaseInitial.firebaseInitialized)
            {
                return;
            }
               
            // Get Current Level 
            int currentLevel = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_LEVEL, 1);
            InternetStatus status = GetNetworkStatus();
            int currentEnergy = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_ENERGY, 1);
            int currentCoin = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_COIN, 1);
            
            var boosterDataS0 = JsonUtility.FromJson<BoosterDatas>(PlayerPrefs.GetString(StringPlayerPrefs.BOOSTER_DATA));


            int is_heart_infinity = 0;
            DateTime infinity = Utills.StringToDate(PlayerPrefs.GetString(StringPlayerPrefs.UNLIMITED_TIME));
            is_heart_infinity = infinity > DateTime.Now ? 1 : 0;

            if (!PlayerPrefs.HasKey(StringPlayerPrefs.IAP_COUNT))
            {
                PlayerPrefs.SetInt(StringPlayerPrefs.IAP_COUNT, 0);
            }
            int iapCount = PlayerPrefs.GetInt(StringPlayerPrefs.IAP_COUNT, 0);
            
            
            // Bắn Properties
            FirebaseAnalytics.SetUserProperty(StringParamFirebase.CurrentLevel, currentLevel.ToString());
            FirebaseAnalytics.SetUserProperty(StringParamFirebase.Internet_Status, status.ToString());
            FirebaseAnalytics.SetUserProperty(StringParamFirebase.IAP_COUNT, iapCount.ToString());
            FirebaseAnalytics.SetUserProperty(StringParamFirebase.Balance_Heart, currentEnergy.ToString());
            FirebaseAnalytics.SetUserProperty(StringParamFirebase.Is_Heart_Infinity, is_heart_infinity.ToString());
            FirebaseAnalytics.SetUserProperty(StringParamFirebase.Balance_Coin, currentCoin.ToString());
            FirebaseAnalytics.SetUserProperty(StringParamFirebase.Balance_Scale_Booster, boosterDataS0.Boosters[0].Amount.ToString());
            FirebaseAnalytics.SetUserProperty(StringParamFirebase.Balance_Magnet_Booster, boosterDataS0.Boosters[1].Amount.ToString());
            FirebaseAnalytics.SetUserProperty(StringParamFirebase.Balance_Location_Booster, boosterDataS0.Boosters[2].Amount.ToString());
            FirebaseAnalytics.SetUserProperty(StringParamFirebase.Balance_Ice_Booster, boosterDataS0.Boosters[3].Amount.ToString());
            
            
            
        }
        
        
        public void LogEventNormal(PositionFirebase pos)
        {
            if (!firebaseInitial.firebaseInitialized)
            {
                Debug.Log("Log Event Fail");
                return;
            }
            Debug.Log("Log Event Success");
            FirebaseAnalytics.LogEvent(
                pos.ToString()
                );
        }
        
        
        public InternetStatus GetNetworkStatus()
        {
            InternetStatus status;
            switch (Application.internetReachability)
            {
                case NetworkReachability.NotReachable:
                    status = InternetStatus.offline;
                    break;
                case NetworkReachability.ReachableViaCarrierDataNetwork:
                    status = InternetStatus.mobile_data;
                    break;
                case NetworkReachability.ReachableViaLocalAreaNetwork:
                    status = InternetStatus.wifi;
                    break;
                default:
                    status = InternetStatus.unknown;
                    break;
            }

            // Gửi sự kiện lên Firebase Analytics
            return status;
        }


       
    }
}