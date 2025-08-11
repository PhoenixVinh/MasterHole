namespace _Scripts.Event
{
    public class CameraFOVEvent
    {
        public delegate void StarLevelEvent();

        public static StarLevelEvent OnStarLevelEvent;
        
        
        
        public delegate void LevelUpEvent(float delayTime);
        public static LevelUpEvent OnLevelUpEvent;
    }
}