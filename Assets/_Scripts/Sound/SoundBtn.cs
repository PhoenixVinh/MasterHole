using _Scripts.Vibration;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _Scripts.Sound
{
    public class SoundBtn : MonoBehaviour
    {
        private Button btn;

        public void Start()
        {
            btn = GetComponent<Button>();
            if (btn == null)
            {
                Debug.LogWarning("SoundBtn is missing Button: " + gameObject.name);
            }
            btn.onClick.AddListener(PlaySound);
        }

        public virtual void PlaySound()
        {
            if(ManagerSound.Instance != null)
                ManagerSound.Instance.PlayEffectSound(EnumEffectSound.ButtonClick);
            ManagerVibration.Instance?.UseVibration(EnumVibration.Light);
        }
    }
}