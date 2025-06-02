using System.Threading.Tasks;
using _Scripts.Event;
using _Scripts.Sound;
using _Scripts.UI;
using _Scripts.UI.WinLossUI;
using UnityEngine;

public class ManagerWinLoss : MonoBehaviour
{
    public GameObject WinUI;
    public GameObject LoseUI;
    public GameObject LevelCoinUI;

    private bool isLevelCoint = false;


   
    public void OnEnable()
    {
        WinLossEvent.OnWin += ShowUIWin;
        WinLossEvent.OnLoss += ShowUILoss;
        
        LevelCointEvent.OnStartLevelCoin += StartLevelCoint;
        LevelCointEvent.OnEndLevelCoin += EndLevelCoint;
        
    }

    private void EndLevelCoint()
    {
        isLevelCoint = false;
    }

    private void StartLevelCoint()
    {
        isLevelCoint = true;
    }


    public void SetLevelCoin()
    {
        this.isLevelCoint = true;
    }

    public void SetNoLevelCoin()
    {
        this.isLevelCoint = false;
    }

    public void OnDisable()
    {
        WinLossEvent.OnWin -= ShowUIWin;
        WinLossEvent.OnLoss -= ShowUILoss;
    }
    
    private async void ShowUIWin()
    {
        await Task.Delay(1000);
        ManagerSound.Instance?.StopAllSoundSFX();
        
        if(ManagerSound.Instance != null)
            ManagerSound.Instance.PlayEffectSound(EnumEffectSound.LevelComplete);
        //LevelCoinUI.GetComponent<WinUI>().SetData(75);
        WinUI.GetComponent<WinUI>().SetData(75);
        int currentLevel = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_LEVEL);
        PlayerPrefs.SetInt(StringPlayerPrefs.CURRENT_LEVEL, currentLevel + 1);
       
    }
    private async void ShowUILoss()
    {
   
        ManagerSound.Instance?.StopAllSoundSFX();
        if (isLevelCoint)
        {
            int coinGet = LevelCointEvent.OnLevelCoinGet.Invoke();
            WinUI.GetComponent<WinUI>().SetData(coinGet);
            //LevelCoinUI.SetActive(true);
            int currentLevel = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_LEVEL);
            PlayerPrefs.SetInt(StringPlayerPrefs.CURRENT_LEVEL, currentLevel + 1);
            return;

        }
        
        
        if (WinUI.activeSelf) return;
        
        
        if(ManagerSound.Instance != null)
            ManagerSound.Instance.PlayEffectSound(EnumEffectSound.FailedLevel);
        
        LoseUI.SetActive(true);
    }

   

        
}