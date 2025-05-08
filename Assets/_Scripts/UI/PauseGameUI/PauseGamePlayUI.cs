using System;
using _Scripts.ManagerScene;
using _Scripts.ManagerScene.HomeScene;
using _Scripts.UI.PauseGameUI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseGamePlayUI: PauseGame
{
    public Button exitButton;
    public Button continueButton;
    public Button quitButton;
    public Button settingButton;
    public GameObject SettingUI;

    private void Start()
    {
        exitButton.onClick.AddListener(CloseUI);
        quitButton.onClick.AddListener(QuitGame);
        continueButton.onClick.AddListener(CloseUI);
        
    }

    private void QuitGame()
    {
         this.OnDisable();
         SceneManager.LoadScene(EnumScene.HomeScene.ToString());
         ManagerHomeScene.Instance.ShowLoseGameUI();
         // Show popup minus health 

    }
    
    

    private void CloseUI()
    {
        this.gameObject.SetActive(false);
    }
    
}