using System;
using System.Collections.Generic;
using _Scripts.Sound;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI.PopupUI.RateUsPopupUI
{
    public class RateUsController : MonoBehaviour
    {
        [SerializeField]private Sprite On;
        [SerializeField]private Sprite Off;
        
        
        public List<Button> RateUsButtons;

        public int amountStar = 0;

        [SerializeField] private Button RateBtn;
        
        


        public void OnEnable()
        {
            amountStar = 5;
            SetUI();
            for(int i = 0; i < RateUsButtons.Count; i++)   
            {
                int index = i;
                RateUsButtons[i].onClick.AddListener(() => OnClick(index));
            }
            RateBtn.onClick.AddListener(RattingApp);

        }

        private void RattingApp()
        {
            
            if(amountStar < 3)
            {
                // Hiển thị thông báo yêu cầu đánh giá lại
                Debug.Log("Please rate us with at least 3 stars.");
                PlayerPrefs.SetInt(StringPlayerPrefs.IS_RATE_US, 1);
                PlayerPrefs.Save();
                this.gameObject.SetActive(false);
                return;
            }
            // Mở liên kết đánh giá ứng dụng trên cửa hàng
            
            
#if UNITY_ANDROID
            Application.OpenURL("https://play.google.com/store/apps/details?id=com.holemaster.eatthemall.holeiomaster");
#elif UNITY_IPHONE
            Application.OpenURL("itms-apps://itunes.apple.com/app/idYOUR_APP_ID");
#endif

            // Lưu trạng thái đã đánh giá
            PlayerPrefs.SetInt(StringPlayerPrefs.IS_RATE_US, 1);
            PlayerPrefs.Save();
            this.gameObject.SetActive(false);
        }

        public void OnDisable()
        {
            for (int i = 0; i < RateUsButtons.Count; i++)
            {
                RateUsButtons[i].onClick.RemoveAllListeners();
            }
            
        }

        private void OnClick(int index)
        {
            ManagerSound.Instance?.PlayEffectSound(EnumEffectSound.BoosterClick);
            amountStar = index + 1;
            SetUI();
            Debug.Log("You clicked on star " + (index + 1));
          
        }


        public void SetUI()
        {
            for (int i = 0; i <= amountStar - 1; i++)
            {
                RateUsButtons[i].image.sprite = On;
            }
            if(amountStar == 5) return;
            for(int i = amountStar; i < RateUsButtons.Count; i++)
            {
                RateUsButtons[i].image.sprite = Off;
            }
        }
        
        
    }
}