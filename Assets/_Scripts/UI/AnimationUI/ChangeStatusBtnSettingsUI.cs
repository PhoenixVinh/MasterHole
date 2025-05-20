using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI.AnimationUI
{
    public class ChangeStatusBtnSettingsUI : MonoBehaviour
    {
        [Header("Sprite")]
        public Sprite BGOn;
        public Sprite BGOff;


        public Sprite ImageOn;
        public Sprite ImageOff;

        [Header("Image Display")]
        public Image ImageDisplay;
        
        private Button _btn;
        
        
        [HideInInspector] public bool _isActive = true;
        private void OnEnable()
        {
            _btn = GetComponent<Button>();
            //_btn.onClick.AddListener(SetStatus);
        }

        public void SetUI()
        {
            if(_isActive) SetStatusOn();
            else SetStatusOff();
        }

        public void SetActive(bool isActive)
        {
            _isActive = isActive;
            SetUI();
        }
        public void SetStatus()
        {
            _isActive = !_isActive;
            SetUI();
        }

        public void SetStatusOn()
        {
            _btn.image.sprite = BGOn;
            ImageDisplay.sprite = ImageOn;
        }

        public void SetStatusOff()
        {
            _btn.image.sprite = BGOff;
            ImageDisplay.sprite = ImageOff;
        }
    }
}