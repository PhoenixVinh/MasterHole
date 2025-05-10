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
            btn.onClick.AddListener(PlaySound);
        }

        private void PlaySound()
        {
            ManagerSound.Instance.PlayEffectSound(EnumEffectSound.ButtonClick);
        }
    }
}