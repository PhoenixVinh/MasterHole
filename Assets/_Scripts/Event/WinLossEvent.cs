namespace _Scripts.Event
{
    public class WinLossEvent
    {
        public delegate void WinEvent();
        public static WinEvent OnWin;


        public delegate void LossEvent();
        public static LossEvent OnLoss;
    }
}