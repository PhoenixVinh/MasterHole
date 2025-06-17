using _Scripts.Event;
using _Scripts.Firebase;
using _Scripts.ManagerScene;
using _Scripts.ManagerScene.HomeScene;
using _Scripts.UI.PopupUI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Scripts.UI.PauseGameUI
{
    public class QuitGamePlayUI : PauseGame
    {
        public Button continueButton;
        public Button quitButton;
        public Button exitButton;
        
        private void Start()
        {
            exitButton.onClick.AddListener(CloseUI);
            quitButton.onClick.AddListener(QuitGame);
            continueButton.onClick.AddListener(CloseUI);
       
        }

        private void QuitGame()
        {




            CloseUI();
            //ResourceEvent.OnUpdateResource?.Invoke();
            ManagerPopup.Instance?.TurnOffPopup();
            MaxAdsManager.Instance?.ShowInterAdsByLevel();
            
           // ManagerFirebase.Instance?.LogLevelEnd(LevelResult.quit, LoseBy.Null);
            //Resource.Instance?.MinusHealth();
            SceneManager.LoadScene(EnumScene.HomeScene.ToString());
          //  ManagerFirebase.Instance?.ChangePositionFirebase(PositionFirebase.home);
            ManagerHomeScene.Instance?.MinusHealth();
            
            
            int currentLoseIndex = PlayerPrefs.GetInt(StringPlayerPrefs.LOSE_INDEX);
            currentLoseIndex++;
            PlayerPrefs.SetInt(StringPlayerPrefs.LOSE_INDEX, currentLoseIndex);
            
            // Show popup minus health 

        }
    
    

        private void CloseUI()
        {
            this.gameObject.SetActive(false);
        }
    }
}