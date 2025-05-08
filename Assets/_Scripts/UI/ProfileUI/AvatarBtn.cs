using System;
using _Scripts.Event;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI.ProfileUI
{
    public class AvatarBtn : MonoBehaviour
    {
        [SerializeField] private Sprite isSelectedImage;
        [SerializeField] private Sprite normalImage;
        
        
        
        public GameObject check;


        private Image image;
        private bool isSelected = false;

        private void Awake()
        {
            image = GetComponent<Image>();
        }

        public void OnEnable()
        {
            ProfileEvent.OnAvateSlected += TurnOff;
        }

        public void OnDisable()
        {
            ProfileEvent.OnAvateSlected -= TurnOff;
        }

        private void TurnOff()
        {
            if (isSelected)
            {
                image.sprite = normalImage;
                check.SetActive(false);
                isSelected = false;
            }
        }

        public void ChangeStatus(bool isSelect)
        {
            
            ProfileEvent.OnAvateSlected?.Invoke();
            if (isSelect)
            {
                image.sprite = isSelectedImage;
                check.SetActive(true);
                isSelected = true;
            }
            else
            {
                image.sprite = normalImage;
                check.SetActive(false);
            }
            
        }
        
        
        
    }
}