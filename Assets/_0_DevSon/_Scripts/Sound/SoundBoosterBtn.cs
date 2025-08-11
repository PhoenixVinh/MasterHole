namespace _Scripts.Sound
{
    public class SoundBoosterBtn : SoundBtn
    {
        public override void PlaySound()
        {
            if(ManagerSound.Instance != null)
                ManagerSound.Instance.PlayEffectSound(EnumEffectSound.BoosterClick);
            //ManagerVibration.Instance?.UseVibration(EnumVibration.Light);
        }
    }
}