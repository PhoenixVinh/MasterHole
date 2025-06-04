using System;
using System.Collections.Generic;
using System.Dynamic;
using _Scripts.Event;
using TMPro;
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
        
        public TMP_InputField inputText;
        
        public ProfileHomeScene profileHomeScene;


        private void OnEnable()
        {
            if (PlayerPrefs.HasKey(StringPlayerPrefs.CURRENT_INDEX_PROFILE))
            {
                PlayerPrefs.SetInt(StringPlayerPrefs.CURRENT_INDEX_PROFILE, 0);
            }
            indexSelected = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_INDEX_PROFILE, 0);
            imageAvatar.sprite = avatarIcons[indexSelected];
        }

        private void Start()
        {
          
            
            SetData();
            saveButton.onClick.AddListener(SaveProfile);
            avatarBtns[indexSelected].gameObject.GetComponent<AvatarBtn>().ChangeStatus(true);
            inputText.text = PlayerPrefs.GetString(StringPlayerPrefs.NAME_PLAYER, "????");
        }

        private void SaveProfile()
        {
            PlayerPrefs.SetInt(StringPlayerPrefs.CURRENT_INDEX_PROFILE, indexSelected);
            PlayerPrefs.SetString(StringPlayerPrefs.NAME_PLAYER, inputText.text);
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