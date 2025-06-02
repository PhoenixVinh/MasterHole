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
            ManagerSound.Instance.PlayEffectSound(EnumEffectSound.ButtonClick);
            ManagerVibration.Instance?.UseVibration(EnumVibration.Light);
         
            int currentEnergy = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_ENERGY, 0);
          
            if (currentEnergy == 0) return;
            ManagerHomeScene.Instance.ShowLoadingUI();
            base.ChangeScene();
          
        }
    }
}