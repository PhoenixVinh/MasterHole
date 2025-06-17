using System.Threading.Tasks;
using _Scripts.Event;
using _Scripts.Firebase;
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

    public GameObject settings;
    public GameObject shop;
   
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
        ManagerFirebase.Instance?.LogLevelEnd(LevelResult.win, LoseBy.Null);
        
        await Task.Delay(1000);
        ManagerSound.Instance?.StopAllSoundSFX();
        
        if(ManagerSound.Instance != null)
            ManagerSound.Instance.PlayEffectSound(EnumEffectSound.LevelComplete);
        ManagerSound.Instance?.PlayEffectSound(EnumEffectSound.Victory);
        //LevelCoinUI.GetComponent<WinUI>().SetData(75);
        WinUI.GetComponent<WinUI>().SetData(75);
        settings.SetActive(false);
        shop.SetActive(false);
        
       
    }
    private async void ShowUILoss()
    {
   
        
        ManagerSound.Instance?.StopAllSoundSFX();
      
        if (isLevelCoint)
        {
            int coinGet = LevelCointEvent.OnLevelCoinGet.Invoke();
            WinUI.GetComponent<WinUI>().SetData(coinGet);
            settings.SetActive(false);
            //LevelCoinUI.SetActive(true);
            //int currentLevel = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_LEVEL);
            //PlayerPrefs.SetInt(StringPlayerPrefs.CURRENT_LEVEL, currentLevel + 1);
            return;

        }
        
        
        if (WinUI.activeSelf) return;
        
        
        if(ManagerSound.Instance != null)
            ManagerSound.Instance.PlayEffectSound(EnumEffectSound.FailedLevel);
        
        LoseUI.SetActive(true);
        settings.SetActive(false);
        shop.SetActive(false);
    }

   

        
}