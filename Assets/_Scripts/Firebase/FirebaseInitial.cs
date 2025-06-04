using Firebase;
using Firebase.Analytics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
   

    
    private void Awake()
    {
        CoinStart = PlayerPrefs.GetInt("CoinStart", 100);

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

