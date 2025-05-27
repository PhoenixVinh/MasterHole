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
        ManagerSound.Instance?.StopEffectSound(EnumEffectSound.TimeEnd);
        if(ManagerSound.Instance != null)
            ManagerSound.Instance.PlayEffectSound(EnumEffectSound.LevelComplete);
        
        WinUI.SetActive(true);
        int currentLevel = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_LEVEL);
        PlayerPrefs.SetInt(StringPlayerPrefs.CURRENT_LEVEL, currentLevel + 1);
       
    }
    private async void ShowUILoss()
    {
        await Task.Delay(1000);
        if (isLevelCoint)
        {
            int coinGet = LevelCointEvent.OnLevelCoinGet.Invoke();
            LevelCoinUI.GetComponent<WinUI>().SetData(coinGet);
            LevelCoinUI.SetActive(true);
            return;

        }
        
        
        if (WinUI.activeSelf) return;
        
        
        if(ManagerSound.Instance != null)
            ManagerSound.Instance.PlayEffectSound(EnumEffectSound.FailedLevel);
        
        LoseUI.SetActive(true);
    }

   

        
}