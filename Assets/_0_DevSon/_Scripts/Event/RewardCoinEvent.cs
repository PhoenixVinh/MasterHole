namespace _Scripts.Event
{
    public class RewardCoinEvent
    {
        public delegate void RewardCoinEventDelegate(int startCoin, int endCoin);
        public static RewardCoinEventDelegate OnRewardCoin;
    }
}