using _Scripts.Sound;
using _Scripts.Vibration;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI.PauseGameUI
{
    public class SettingGamePlayUI : PauseGame
    {

        [Header("Variable Vibration")] 
        public Sprite VibrationOn;
        public Sprite VibrationOff;
        public Image VibrationImage;
        
        private bool canVibrate = true;
        
        [Header("Variable Sound")]
        public Sprite SoundOn;
        public Sprite SoundOff;
        public Image SoundImage;
        private bool canSound = true;
        
        
        [Header("Sprites Button")]
        public Sprite ButtonOn;
        public Sprite ButtonOff;
        
        
        
        public Button VibrationBtn;
        public Button SoundBtn;

        public override void OnEnable()
        {
            base.OnEnable();
            canVibrate = PlayerPrefs.GetInt(StringPlayerPrefs.USE_VIBRATION) == 1;
            canSound = PlayerPrefs.GetInt(StringPlayerPrefs.USE_BGMUSIC) == 1;
          

            SetUI();
            
            VibrationBtn.onClick.AddListener(ChangeStatusVibration);
            SoundBtn.onClick.AddListener(ChangeStatusSound);
        }

        public override void OnDisable()
        {
            base.OnDisable();
            VibrationBtn.onClick.RemoveAllListeners();
            SoundBtn.onClick.RemoveAllListeners();
            
            
        }
        private void ChangeStatusSound()
        {
            canSound = !canSound;
            if (ManagerSound.Instance != null)
            {
                ManagerSound.Instance.SetSound(canSound);   
            }
            SetUI();
        }

        private void ChangeStatusVibration()
        {
            canVibrate = !canVibrate;
            if (ManagerVibration.Instance != null)
            {
                ManagerVibration.Instance.SetVibration(canVibrate);
            }
            SetUI();
            
        }

        private void SetUI()
        {
            if (canVibrate)
            {
                VibrationImage.sprite = VibrationOn;
                VibrationBtn.image.sprite = ButtonOn;
            }
            else
            {
                VibrationImage.sprite = VibrationOff;
                VibrationBtn.image.sprite = ButtonOff;
            }

            if (canSound)
            {
                SoundImage.sprite = SoundOn;
                SoundBtn.image.sprite = ButtonOn;
            }
            else
            {
                SoundImage.sprite = SoundOff;
                SoundBtn.image.sprite = ButtonOff;
            }
            
            
        }
    }
}