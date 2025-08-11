namespace _Scripts.Event
{
    public class TimeEvent
    {
        public delegate void FreezeTimeEvent(float time); 
        public static FreezeTimeEvent OnFreezeTime;
    }
}