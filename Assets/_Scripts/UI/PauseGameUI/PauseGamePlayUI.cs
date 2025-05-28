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
   
   
    public TMP_Text levelText;


    public void OnEnable()
    {
        base.OnEnable();    
        int level = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_LEVEL, 1);
        this.levelText.text = $"LEVEL{level}";
    }
    private void Start()
    {
        exitButton.onClick.AddListener(CloseUI);
      
        continueButton.onClick.AddListener(CloseUI);
       
    }

   
    
    

    private void CloseUI()
    {
        this.gameObject.SetActive(false);
    }
    
}