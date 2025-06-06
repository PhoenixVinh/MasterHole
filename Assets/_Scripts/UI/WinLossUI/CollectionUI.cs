

using System;
using System.Threading.Tasks;
using _Scripts.Data.CollectionData;
using _Scripts.ManagerScene;
using _Scripts.ManagerScene.HomeScene;
using _Scripts.UI.PauseGameUI;
using _Scripts.UI.WinLossUI.SkinCollectionUI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



namespace _Scripts.UI.WinLossUI
{
    public class CollectionUI : PauseGame
    {
        public CollectionSO collection;
        public SkinSO skin;

        public Image image;

        public GameObject content;
        public Button continueButton;
        public Button homeBtn;

        public SkinProcess SkinProcess;

        
        public override void OnEnable()
        {
          
            continueButton.onClick.AddListener(ShowNextlevel);
            homeBtn.onClick.AddListener(ChangeHomeScene);
        }


        public bool CanShowContent(int level)
        {

            foreach (var i in collection.ItemCollectionData)
            {
                if (level == i.LevelUnlock)
                {
                    image.sprite = i.image;
                    return true;
                }
            }

            foreach (var i in skin.skins)
            {
                if (level == i.levelUnlock)
                {
                    image.sprite = i.image;
                    return true;
                }
            }
            

            return false;
        }

        public async void ShowContent(int level)
        {   
            
            
            
            
            this.content.SetActive(true);
            

            if (level == 4)
            {
                this.homeBtn.gameObject.SetActive(false);
                
                this.continueButton.onClick.RemoveAllListeners();
                this.continueButton.onClick.AddListener(ShowTutorialSkin);
                //SceneManager.LoadScene(EnumScene.HomeScene.ToString());
            }
            else
            {
                this.continueButton.gameObject.SetActive(true);
                this.homeBtn.gameObject.SetActive(true);
            }
        }

        public void FixedUpdate()
        {
            
        }

        private void ShowTutorialSkin()
        {
            SceneManager.LoadScene(EnumScene.HomeScene.ToString());
            this.content.SetActive(false);
            ManagerHomeScene.Instance?.LoadTutorial();
        }

        private void ChangeHomeScene()
        {
            
            SceneManager.LoadScene(EnumScene.HomeScene.ToString());
            
        }
        
        private void ShowNextlevel()
        {

            int currentLevel = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_LEVEL);
            
            this.content.SetActive(false);
            if (SkinProcess.GetTarget(currentLevel) != -1)
            {
                SkinProcess.gameObject.SetActive(true);
            }
            else
            {
                ManagerLevelGamePlay.Instance.LoadNextLevel();
            }
           
           
        }

 
    }
}