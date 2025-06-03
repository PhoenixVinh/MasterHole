using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Booster
{
    [Serializable]
    [CreateAssetMenu(fileName = "BoosterData", menuName = "Data/BoosterData", order = 1)]
    public class BoosterDataSO: ScriptableObject
    {
        public List<BoosterData> Boosters;
    }

    
    [Serializable]
    public class BoosterDatas
    {
        public List<BoosterData> Boosters;
    }
    
    
}