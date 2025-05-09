using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Data.CollectionData
{
    [CreateAssetMenu(fileName = "DataCollection", menuName = "Data/DataCollection")]
    public class CollectionSO : ScriptableObject
    {
        public List<ItemCollectionData> ItemCollectionData;
    }
}