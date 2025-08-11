using UnityEngine;

namespace _Scripts.Sound
{
    public class ShowSoundDisplay : MonoBehaviour
    {
        public EnumEffectSound Sound;

        public void OnEnable()
        {
            ManagerSound.Instance?.PlayEffectSound(Sound);
        }
    }
}