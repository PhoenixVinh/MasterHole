using System;
using _Scripts.Booster;
using _Scripts.UI;
using Firebase.Analytics;
using UnityEngine;

namespace _Scripts.Firebase
{
    public class ManagerFirebase : MonoBehaviour
    {
        [SerializeField]private FirebaseInitial firebaseInitial;
        public static ManagerFirebase Instance;
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


        public void LogEventWithString(string eventName)
        {
            if (firebaseInitial.firebaseInitialized)
                FirebaseAnalytics.LogEvent(eventName);
        }
        
        public void LogLvelStart() // MẪu, sau làm các khác dựa vào hàm này
        {
            if (firebaseInitial.firebaseInitialized)
            {
                return;
            }
            //
            // FirebaseAnalytics.LogEvent(
            //     "level_start",
            //     new Parameter("level", _playinglevel),
            //     new Parameter("play_type", currentPlayType),
            //     new Parameter("total_duration_start", total_duration_start),
            //     new Parameter("play_index", _playingIndexOfLevel),
            //     new Parameter("lose_index", lose_streak)
            // );
        }


        public void LogLevelStart(int playerLevel, PlayType playType, int msec, int play_index, int lose_index )
        {
            if (firebaseInitial.firebaseInitialized)
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

        public void LogLevelEnd()
        {
            
        }
        
       
        
        
        
        
        
        

        public void SetPlayerProperties()
        {
            if (firebaseInitial.firebaseInitialized)
            {
          
                return;
            }
               
            // Get Current Level 
            int currentLevel = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_LEVEL, 1);
            InternetStatus status = GetNetworkStatus();
            int currentEnergy = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_ENERGY, 1);
            int currentCoin = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_COIN, 1);
            
            var boosterDataS0 = JsonUtility.FromJson<BoosterDatas>(PlayerPrefs.GetString(StringPlayerPrefs.BOOSTER_DATA));
            
            
            // Bắn Properties
            FirebaseAnalytics.SetUserProperty(StringParamFirebase.CurrentLevel, currentLevel.ToString());
            FirebaseAnalytics.SetUserProperty(StringParamFirebase.Internet_Status, status.ToString());// thay 0 thành số tiền
            FirebaseAnalytics.SetUserProperty(StringParamFirebase.Balance_Heart, currentEnergy.ToString());
            FirebaseAnalytics.SetUserProperty(StringParamFirebase.Balance_Coin, currentCoin.ToString());
            FirebaseAnalytics.SetUserProperty(StringParamFirebase.Balance_Scale_Booster, boosterDataS0.Boosters[0].Amount.ToString());
            FirebaseAnalytics.SetUserProperty(StringParamFirebase.Balance_Magnet_Booster, boosterDataS0.Boosters[1].Amount.ToString());
            FirebaseAnalytics.SetUserProperty(StringParamFirebase.Balance_Location_Booster, boosterDataS0.Boosters[2].Amount.ToString());
            FirebaseAnalytics.SetUserProperty(StringParamFirebase.Balance_Ice_Booster, boosterDataS0.Boosters[3].Amount.ToString());
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