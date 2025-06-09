using System;
using _Scripts.Firebase;
using _Scripts.ManagerScene.HomeScene;
using _Scripts.Sound;
using _Scripts.Vibration;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace _Scripts.UI.HomeSceneUI.ButtonUI
{
    public class PlayBtn : ChangeSceneBtn
    {
       
        
        public override void ChangeScene()
        {
            ManagerSound.Instance?.PlayEffectSound(EnumEffectSound.ButtonClick);
            ManagerVibration.Instance?.UseVibration(EnumVibration.Light);


            DateTime infinity = Utills.StringToDate(PlayerPrefs.GetString(StringPlayerPrefs.UNLIMITED_TIME));
            if (infinity < DateTime.Now)
            {
                int currentEnergy = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_ENERGY, 0);
                if (currentEnergy == 0) return;
            }
            
            ManagerFirebase.Instance?.ChangePositionFirebase(PositionFirebase.ingame);
            ManagerHomeScene.Instance.ShowLoadingUI();
            base.ChangeScene();
            
            
          
        }
        
        
        
    }
}