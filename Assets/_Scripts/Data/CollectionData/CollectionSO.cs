using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Data.CollectionData
{
    [CreateAssetMenu(fileName = "DataCollection", menuName = "Data/DataCollection")]
    public class CollectionSO : ScriptableObject
    {
        public List<ItemCollectionData> ItemCollectionData;



        public int GetLevelCollection(int level)
        {
            int result = 0;
            for (int i = 0; i < ItemCollectionData.Count; i++)
            {
                if (level <= ItemCollectionData[i].LevelUnlock)
                {
                    break;
                }
                result++;
            }
            Debug.Log(result);
            return result;
        }
    }
}