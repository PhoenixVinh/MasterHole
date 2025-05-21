using System;
using _Scripts.ManagerScene;
using _Scripts.ManagerScene.HomeScene;
using _Scripts.UI;
using _Scripts.UI.PauseGameUI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseGamePlayUI: PauseGame
{
    public Button exitButton;
    public Button continueButton;
    public Button quitButton;
   
    public TMP_Text levelText;


    private void OnEnable()
    {
        int level = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_LEVEL, 1);
        this.levelText.text = $"LEVEL{level}";
    }
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