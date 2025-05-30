
using _Scripts.UI;
using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using UnityEngine;

public class FirebaseSetup : MonoBehaviour
{
    private FirebaseApp app;
    private void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available) {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                app = Firebase.FirebaseApp.DefaultInstance;
           
    
                // Set a flag here to indicate whether Firebase is ready to use by your app.
            } else {
                UnityEngine.Debug.LogError(System.String.Format(
                    "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                
                // Firebase Unity SDK is not safe to use here.
            }
        });
        TestLogin();

    }

    
    [ContextMenu("TEST LOGIN")]
    public void TestLogin()
    {


        int currentLevel = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_LEVEL, 1);
        FirebaseAnalytics.LogEvent(
            "User Properties",
            new Parameter("Current_level", currentLevel)
            );
        
    
    }
    
    public void OnDestroy()
    {
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelEnd);
    }
}
