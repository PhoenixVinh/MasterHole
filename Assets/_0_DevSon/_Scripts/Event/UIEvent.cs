using System.Collections.Generic;
using _Scripts.UI.HomeSceneUI.ShopUI.TreasureUI;

namespace _Scripts.Event
{
    public class UIEvent
    {
        public delegate void RewardEvent(List<DataReward> datas);
        public static  RewardEvent OnRewardedSuccess;
    }
}