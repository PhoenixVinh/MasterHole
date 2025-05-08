using System;
using System.Collections.Generic;
using System.Dynamic;
using _Scripts.Event;
using UnityEngine;
using UnityEngine.UI;


namespace _Scripts.UI.ProfileUI
{
    public class ManagerProfile : MonoBehaviour
    {
        public List<Button> avatarBtns;
        
        public List<Sprite> avatarIcons;

        public Image imageAvatar;
        
        public Button saveButton;
        
        private int indexSelected = 0;
        
        public ProfileHomeScene profileHomeScene;
        private void Start()
        {
          
            indexSelected = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_INDEX_PROFILE, 0);
            SetData();
            saveButton.onClick.AddListener(SaveProfile);
            avatarBtns[indexSelected].gameObject.GetComponent<AvatarBtn>().ChangeStatus(true);
        }

        private void SaveProfile()
        {
            PlayerPrefs.SetInt(StringPlayerPrefs.CURRENT_INDEX_PROFILE, indexSelected);
            this.gameObject.SetActive(false);
        }


        private void SetData()
        {
           
            for (int i = 0; i < avatarBtns.Count; i++)
            {
                int indexBtn = i;
                avatarBtns[i].onClick.AddListener(() => SetIndexButton(indexBtn));
               
            }
        }

        
        private void SetIndexButton(int indexBtn)
        {
            indexSelected = indexBtn;
            imageAvatar.sprite = avatarIcons[indexBtn];
        }

        public void OnDisable()
        {
            profileHomeScene.UpdateProfile();
        }
        
       
    }
}