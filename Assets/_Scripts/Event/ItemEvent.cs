using UnityEngine;

namespace _Scripts.Event
{
    public class ItemEvent
    {
        public delegate void AddScore(int score);
        public static AddScore OnAddScore;




        public delegate void ItemMission(EnumItem item);
        public static ItemMission OnItemMission;
        

        public delegate void ItemMissionFinding(GameObject item);
        public static ItemMissionFinding OnItemMissionFinding;
    }
}