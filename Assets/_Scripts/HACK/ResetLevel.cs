using _Scripts.Event;
using _Scripts.Sound;
using _Scripts.UI;
using _Scripts.UI.HomeSceneUI.ResourcesUI;
using UnityEngine;

namespace _Scripts.HACK
{
    public class ResetLevel: MonoBehaviour
    {
        public EnumEffectSound effectSound;
        public void ResetLevelGamePlay()
        {
            ManagerSound.Instance.PlayEffectSound(effectSound);
            PlayerPrefs.SetInt(StringPlayerPrefs.TUTORIAL_LEVEL_3, 0);
            Resource.Instance.AddMaxHealth();
            Resource.Instance.AddMaxCoin();
            PlayerPrefs.SetInt(StringPlayerPrefs.CURRENT_LEVEL, 1);
          
            PlayerPrefs.Save();
        }
    }
}