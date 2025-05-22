using _Scripts.Event;
using _Scripts.ManagerScene;
using _Scripts.ManagerScene.HomeScene;
using _Scripts.UI.HomeSceneUI.ResourcesUI;
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
            
            
            int currentEnergy = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_ENERGY);
            currentEnergy--;
            PlayerPrefs.SetInt(StringPlayerPrefs.CURRENT_ENERGY, currentEnergy);
            
            
            ResourceEvent.OnUpdateResource?.Invoke();
            //Resource.Instance.MinusHealth();
            SceneManager.LoadScene(EnumScene.HomeScene.ToString());
            this.OnDisable();
            // Show popup minus health 

        }
    
    

        private void CloseUI()
        {
            this.gameObject.SetActive(false);
        }
    }
}