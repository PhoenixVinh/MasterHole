namespace _Scripts.Event
{
    public class LevelCointEvent
    {
        public delegate void LevelCoin();

        public static LevelCoin OnStartLevelCoin;
        public static LevelCoin OnEndLevelCoin;
        
        
        
        public delegate int LevelCoinGet();
        public static LevelCoinGet OnLevelCoinGet;
    }
}