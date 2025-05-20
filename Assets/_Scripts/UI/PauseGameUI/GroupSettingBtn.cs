using System;
using _Scripts.Sound;
using _Scripts.UI.AnimationUI;
using _Scripts.Vibration;
using UnityEngine;

namespace _Scripts.UI.PauseGameUI
{
    public class GroupSettingBtn : MonoBehaviour
    {
        
        public ChangeStatusBtnSettingsUI BgMusicBtn;
        public ChangeStatusBtnSettingsUI sfxSoundBtn;
        public ChangeStatusBtnSettingsUI vibrationBtn;



        private bool canBgMusic = true;
        private bool canSfxSound = true;
        private bool canVibration = true;


        private void Start()
        {
            canBgMusic = PlayerPrefs.GetInt(StringPlayerPrefs.USE_VIBRATION) == 1;
            canSfxSound = PlayerPrefs.GetInt(StringPlayerPrefs.USE_BGMUSIC) == 1;
            canVibration = PlayerPrefs.GetInt(StringPlayerPrefs.USE_VIBRATION) == 1;
            SetUI();
        }


        public void SetUI()
        {
            BgMusicBtn.SetActive(canBgMusic);
            sfxSoundBtn.SetActive(canSfxSound);
            vibrationBtn.SetActive(canVibration);
        }
        public void ChangeStatusVibration()
        {
            canVibration = !canVibration;
            if (ManagerVibration.Instance != null)
            {
                ManagerVibration.Instance.SetVibration(canVibration);
            }
            vibrationBtn.SetActive(canVibration);
            
        }

        public void ChangeStatusMusicBG()
        {
            canBgMusic = !canBgMusic;
            if (ManagerSound.Instance != null)
            {
                ManagerSound.Instance.SetBGMusic(canBgMusic);   
            }
            BgMusicBtn.SetActive(canBgMusic);
           
        }

        public void ChangeStatusSfxSound()
        {
            canSfxSound = !canSfxSound;
            if (ManagerSound.Instance != null)
            {
                ManagerSound.Instance.SetSfxSound(canSfxSound);
            }
            sfxSoundBtn.SetActive(canSfxSound);
        }
        
        
        
        
    }
}