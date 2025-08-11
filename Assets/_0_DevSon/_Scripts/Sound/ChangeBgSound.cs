using System;
using _Scripts.UI;
using UnityEngine;

namespace _Scripts.Sound
{
    public class ChangeBgSound : MonoBehaviour
    {
        public EnumBackgroundSound BGSound;

        public void OnEnable()
        {

            int currentLevel = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_LEVEL, 1);
            if (currentLevel != 1)
            {
                ManagerSound.Instance?.StopAllSoundSFX();
                ManagerSound.Instance?.ChangeBackgroundMusic(BGSound);
            }
           
        }
    }
}