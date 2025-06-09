
using System;
using System.Threading.Tasks;
using _Scripts.Booster;
using _Scripts.Event;
using _Scripts.Firebase;
using _Scripts.ManagerScene.HomeScene;
using _Scripts.Map.MapSpawnItem;
using _Scripts.ObjectPooling;
using _Scripts.Sound;
using _Scripts.Tutorial;
using _Scripts.UI;
using _Scripts.UI.MissionUI;
using UnityEngine;
using UnityEngine.Rendering;



public class ManagerLevelGamePlay : MonoBehaviour
{
    public static ManagerLevelGamePlay Instance;

    public LevelGamePlaySO level;
    public int currentLevel = 1;


    private bool isShowBanner = false;

    private void Awake()
    {
        if(Instance == null) Instance = this;
        else  Destroy(gameObject);
  
        
        currentLevel = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_LEVEL, 1);
        level = ScriptableObject.CreateInstance<LevelGamePlaySO>();
        //ManagerHomeScene.Instance.HideLoadingUI();
    }
   


    private void Start()
    {

        isShowBanner = currentLevel >= ManagerFirebase.Instance?.firebaseInitial.Level_Show_Banner;
        LoadLevelSO();
        if (!PlayerPrefs.HasKey(StringPlayerPrefs.LOSE_INDEX))
        {
            PlayerPrefs.SetInt(StringPlayerPrefs.LOSE_INDEX, 0);
        }
        int currentLose_Index = PlayerPrefs.GetInt(StringPlayerPrefs.LOSE_INDEX, 0);
        
        ManagerFirebase.Instance?.LogLevelStart(currentLevel, PlayType.home, (int)level.timeToComplete*1000, currentLose_Index,currentLose_Index);
        SpawnLevel();
        ManagerTutorial.Instance.ShowTutorials(currentLevel);
        PlayerPrefs.SetInt(StringPlayerPrefs.COUNT_USE_BOOSTER_ICE, 0);
        PlayerPrefs.SetString(StringPlayerPrefs.PLAYER_TYPE, PlayType.home.ToString());
    }
        
    public void ChangeLevel(int level)
    {
        currentLevel = level;
        
        
    }

    public bool LoadLevelSO()
    {

        int loadLevel = currentLevel;
        if (currentLevel > 100)
        {
            loadLevel = 80 + currentLevel%20;
        }
        
        level = Resources.Load<LevelGamePlaySO>($"DataLevelSO/DataLevel_{loadLevel}");
        
        if (level == null)
        {
            Debug.LogError($"No DataLevelSO found in Resources at path: {Application.dataPath}");
            return false;
        }
        return true;
    }

    public async  Task<bool>  SpawnLevel()
    {

        
        PlayerPrefs.SetInt(StringPlayerPrefs.COUNT_USE_BOOSTER_ICE, 0);
        
        if(ManagerHomeScene.Instance != null)
            ManagerHomeScene.Instance.ShowLoadingUI();
        if (ManagerSound.Instance != null)
        {
            ManagerSound.Instance.StopEffectSound(EnumEffectSound.Magnet);
            ManagerSound.Instance.StopAllSoundSFX();
        }
        
        
        
        
        MissionPooling.Instance.DisactiveAllItem();
        HoleController.Instance.Reset();
        HoleController.Instance.SetPosition(Vector3.zero);
        HoleController.Instance.gameObject.SetActive(false);
        
        //Task.Delay(200);
        SpawnItemMap.Instance.SetData(level.levelSpawnData, level.ScoreDatas, level.mapPosition, level.mapScale);
        ManagerMission.Instance.SetData(level.missionData);
        
        //Task.Delay(100);
        ColdownTime.Instance.SetData(level.timeToComplete);
        ManagerBooster.Instance.SetData();
       
        HoleController.Instance.gameObject.SetActive(true);
        CameraFOVEvent.OnStarLevelEvent?.Invoke();
        HoleEvent.OnTurnOffSkillUI?.Invoke();
        //100
        if (ManagerHomeScene.Instance != null)
        {
            ManagerHomeScene.Instance.HideLoadingUI();
        }

        if (isShowBanner)
        {
            MaxAdsManager.Instance?.ShowBannerAd();
        }
        
        return true;

    }


    public void LoadNextLevel()
    {
        
        
        //currentLevel  = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_LEVEL, 1);
        currentLevel = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_LEVEL);
        Debug.Log(currentLevel);
     

      
        if (LoadLevelSO())
        {
            
            ManagerTutorial.Instance.ShowTutorials(currentLevel);
           
            SpawnLevel();
            
            PlayerPrefs.SetInt(StringPlayerPrefs.LOSE_INDEX, 0);
        
            PlayerPrefs.SetString(StringPlayerPrefs.PLAYER_TYPE, PlayType.next.ToString());
            ManagerFirebase.Instance?.LogLevelStart(currentLevel, PlayType.next, (int)level.timeToComplete*1000, 0,0);
            
            
            
        }
        else
        {
            Debug.LogError($"You are at The Max Level");
        }
    }

    public void LoadLevelAgain()
    {
        ManagerTutorial.Instance.ShowTutorials(currentLevel);
        SpawnLevel();
        int currentLose_Index = PlayerPrefs.GetInt(StringPlayerPrefs.LOSE_INDEX, 0);
      
        
        PlayerPrefs.SetString(StringPlayerPrefs.PLAYER_TYPE, PlayType.retry.ToString());
        ManagerFirebase.Instance?.LogLevelStart(currentLevel, PlayType.retry, (int)level.timeToComplete*1000, currentLose_Index,currentLose_Index);

    }

    public void OnDestroy()
    {
        MaxAdsManager.Instance?.HideBannerAd();
    }
}