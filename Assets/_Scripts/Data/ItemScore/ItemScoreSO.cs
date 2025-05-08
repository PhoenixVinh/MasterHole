using System.Collections.Generic;
using UnityEngine;
    
[CreateAssetMenu(fileName = "ItemScore", menuName = "Data/ItemScore")]
public class ItemScoreSO : ScriptableObject
{
    public List<ItemScoreData> itemScoreData;
}
