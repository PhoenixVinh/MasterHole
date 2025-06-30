using Firebase;
using Firebase.Analytics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using _Scripts.UI;
using UnityEngine;

public class FirebaseInitial : MonoBehaviour
{
    const int kMaxLogSize = 16382;
    Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;
    public bool firebaseInitialized = false;
    Firebase.DependencyStatus dependencyStatusFirebase = Firebase.DependencyStatus.UnavailableOther;
  
    
    int retryAttempt = 3;

    //Remote Var

    public int CoinStart = 100;
    public string VersionTest = "";

    public int Level_Show_Inter = 0;
    public int Inter_Distance_Level = 0;
    public int CapingTime_Inter = 0;
    public int Level_Show_Banner = 0;
    
    
    
    
    public float capingtime_inter = 90f;
    public int cmp_begin = 15;
    public int cmp_distance = 5;
    
    private void Awake()
    {
        CoinStart = PlayerPrefs.GetInt("CoinStart", 100);
        Level_Show_Inter = PlayerPrefs.GetInt(StringPlayerPrefs.LEVEL_SHOW_INTER, 20);
        Inter_Distance_Level = PlayerPrefs.GetInt(StringPlayerPrefs.INTER_DISTANCE_LEVEL, 2);
        CapingTime_Inter = PlayerPrefs.GetInt(StringPlayerPrefs.CAPINGTIME_INTER, 120);
        Level_Show_Banner = PlayerPrefs.GetInt(StringPlayerPrefs.LEVEL_SHOW_BANNER, 15);

        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError(
                    "Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    void InitializeFirebase()
    {
        System.Collections.Generic.Dictionary<string, object> defaults =
            new System.Collections.Generic.Dictionary<string, object>();

        defaults.Add("CoinStart", CoinStart);
        

        Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults).ContinueWith(Task =>
        {
            Debug.Log("Remote config ready!");
        }); 
        FetchFireBase();
        InitializeFirebaseLogEvent();
    }
    public void InitializeFirebaseLogEvent()
    {
        FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
        // Set the user ID.
        FirebaseAnalytics.SetUserId(SystemInfo.deviceUniqueIdentifier);
        // Set default session duration values.
        //FirebaseAnalytics.SetMinimumSessionDuration(new TimeSpan(0, 0, 10));
        FirebaseAnalytics.SetSessionTimeoutDuration(new TimeSpan(0, 30, 0));
        firebaseInitialized = true;
        Debug.Log("Initate success firebase");

    }
    public void FetchFireBase()
    {
        FetchDataAsync();
    }
    public Task FetchDataAsync()
    {
        //Debug.Log("Fetching data...");
        // FetchAsync only fetches new data if the current data is older than the provided
        // timespan.  Otherwise it assumes the data is "recent enough", and does nothing.
        // By default the timespan is 12 hours, and for production apps, this is a good
        // number.  For this example though, it's set to a timespan of zero, so that
        // changes in the console will always show up immediately.
        System.Threading.Tasks.Task fetchTask = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.FetchAsync(
            TimeSpan.Zero);
        return fetchTask.ContinueWith(FetchComplete);
    }
    void FetchComplete(Task fetchTask)
    {
        if (fetchTask.IsCanceled)
        {
            Debug.Log("Fetch canceled.");
        }
        else if (fetchTask.IsFaulted)
        {
            Debug.Log("Fetch encountered an error.");
            retryAttempt++;
            double retryDelay = Math.Pow(2, Math.Min(6, retryAttempt));
            Invoke("FetchFireBase", (float)retryDelay);
        }
        else if (fetchTask.IsCompleted)
        {
            Debug.Log("Fetch completed successfully!");
        }

        var info = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.Info;

        Debug.Log(info.LastFetchStatus);
        switch (info.LastFetchStatus)
        {
            case Firebase.RemoteConfig.LastFetchStatus.Success:
                Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.ActivateAsync().ContinueWith(task =>
                {
                    //Debug.Log(String.Format("Remote data loaded and ready (last fetch time {0}).",info.FetchTime));
                   
                    //CoinStart = (int)Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue("CoinStart").LongValue;
                    
                    Level_Show_Inter = (int)Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue("level_show_inter").LongValue;
                    Inter_Distance_Level = (int)Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue("level_distance_level").LongValue;
                    CapingTime_Inter = (int)Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue("capingtime_inter").LongValue;
                    Level_Show_Banner = (int)Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue("level_show_banner").LongValue;
                    cmp_begin = (int)Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue("cmp_begin").LongValue;
                    cmp_distance = (int)Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue("cmp_distance").LongValue;
                    capingtime_inter = (float)Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue("capingtime_inter").DoubleValue;
                    MaxAdsManager.Instance.timeShowInter = capingtime_inter;
                    
                    PlayerPrefs.SetInt(StringPlayerPrefs.LEVEL_SHOW_INTER, Level_Show_Inter);
                    PlayerPrefs.SetInt(StringPlayerPrefs.INTER_DISTANCE_LEVEL, Inter_Distance_Level);
                    PlayerPrefs.SetInt(StringPlayerPrefs.CAPINGTIME_INTER, CapingTime_Inter);
                    PlayerPrefs.SetInt(StringPlayerPrefs.LEVEL_SHOW_BANNER, Level_Show_Banner);
                });
                break;
            case Firebase.RemoteConfig.LastFetchStatus.Failure:
                switch (info.LastFetchFailureReason)
                {
                    case Firebase.RemoteConfig.FetchFailureReason.Error:
                        Debug.Log("Fetch failed for unknown reason");
                        break;
                    case Firebase.RemoteConfig.FetchFailureReason.Throttled:
                        Debug.Log("Fetch throttled until " + info.ThrottledEndTime);
                        break;
                }
                break;
            case Firebase.RemoteConfig.LastFetchStatus.Pending:
                Debug.Log("Latest Fetch call still pending.");
                break;
        }
    }
    
}

