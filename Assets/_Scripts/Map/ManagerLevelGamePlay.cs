
using System.Threading.Tasks;
using _Scripts.Booster;
using _Scripts.Event;
using _Scripts.Map.MapSpawnItem;
using _Scripts.ObjectPooling;
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

    private void Awake()
    {
        if(Instance == null) Instance = this;
        else  Destroy(gameObject);
  
        
        currentLevel = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_LEVEL, 1);
        level = ScriptableObject.CreateInstance<LevelGamePlaySO>();
       
    }
   


    private void Start()
    {
       
        LoadLevelSO();
        SpawnLevel();
        ManagerTutorial.Instance.ShowTutorials(currentLevel);
        
       
       
    }

    public void ChangeLevel(int level)
    {
        currentLevel = level;
        
        
    }

    public bool LoadLevelSO()
    {
        
        level = Resources.Load<LevelGamePlaySO>($"DataLevelSO/DataLevel_{currentLevel}");
        if (level == null)
        {
            Debug.LogError($"No DataLevelSO found in Resources at path: {Application.dataPath}");
            return false;
        }
        return true;
    }

    public Task<bool>  SpawnLevel()
    {
        
        
        
        MissionPooling.Instance.DisactiveAllItem();
        HoleController.Instance.Reset();
        HoleController.Instance.SetPosition(Vector3.zero);
        HoleController.Instance.gameObject.SetActive(false);
        
        Task.Delay(200);
        SpawnItemMap.Instance.SetData(level.levelSpawnData, level.ScoreDatas, level.mapPosition, level.mapScale);
        ManagerMission.Instance.SetData(level.missionData);
        
        Task.Delay(100);
        ColdownTime.Instance.SetData(level.timeToComplete);
        ManagerBooster.Instance.SetData();
        HoleController.Instance.gameObject.SetActive(true);
        CameraFOVEvent.OnStarLevelEvent?.Invoke();

        return Task.FromResult(true);

    }


    public void LoadNextLevel()
    {
        currentLevel  = currentLevel + 1;
        if (LoadLevelSO())
        {
            ManagerTutorial.Instance.ShowTutorials(currentLevel);
            SpawnLevel();
            
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
    }
}