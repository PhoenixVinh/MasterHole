using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class MissionData
{
    public string idItem;
    public int AmountItems;
    public Sprite image;
}

[CreateAssetMenu(fileName = "Mission", menuName = "Data/Mission")]
public class MissionSO : ScriptableObject
{
    public List<MissionData> misstionsData;
}