

using _Scripts.Data.CollectionData;
using _Scripts.ManagerScene;

using _Scripts.UI.PauseGameUI;
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

        public void ShowContent()
        {
            this.content.SetActive(true);
        }
        
        private void ChangeHomeScene()
        {
            SceneManager.LoadScene(EnumScene.HomeScene.ToString());
            
        }
        
        private void ShowNextlevel()
        {
            
            this.content.SetActive(false);
            ManagerLevelGamePlay.Instance.LoadNextLevel();
           
        }
        

    }
}