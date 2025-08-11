
using System;
using UnityEngine;

namespace HoleLevelData
{
    
    [Serializable]
    public class DataLevel
    {
        public int levelId = 1;
        public int amountExp = 3;
        public float radious = 5f;
    }


    [CreateAssetMenu(fileName = "Level Hole", menuName = "Data/Level Hole")]
    public class LevelHoleSO : ScriptableObject
    {
        public DataLevel[] levels;
    }
}


