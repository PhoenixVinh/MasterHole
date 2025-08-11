namespace _Scripts.Firebase
{
    public class LevelEndData
    {
        public int PlayerLevel;
        public string PlayerType;
        public int TimeStart;
        public int TimeEnd;
        public int RemainDuration;
        public int TimeDuration => TimeEnd - RemainDuration;
        public int PlayIndex;
        public int LoseIndex;
        public int TotalFood;
        public int ClearedFood;
        public string LevelResult;
        public string LoseBy;
    }
}