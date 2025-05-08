using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Booster
{
    [CreateAssetMenu(fileName = "BoosterData", menuName = "Data/BoosterData", order = 1)]
    public class BoosterDataSO: ScriptableObject
    {
        public List<BoosterData> Boosters;
    }
}