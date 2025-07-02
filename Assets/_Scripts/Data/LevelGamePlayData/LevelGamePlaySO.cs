using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "Data/Level Game Play Data")]
public class LevelGamePlaySO : ScriptableObject
{
    public LevelSpawnData levelSpawnData;
    public MissionSO missionData;
    public float timeToComplete ;
    public List<ItemScoreData> ScoreDatas;




    public Vector3 mapPosition = new Vector3(0f,-11f,0f);
    public Vector3 mapScale = new Vector3(10000f,10000f,10000f);
    [ContextMenu("Update Score Data")]
    public void UpdateData()
    {
        ScoreDatas = new List<ItemScoreData>();
        var listItem = levelSpawnData.listItemSpawns;
        HashSet<string> names = new HashSet<string>();
        foreach (var itemSpawn in listItem)
        {
            if (!names.Contains(itemSpawn.id))
            {
                names.Add(itemSpawn.id);
            }
        }


        foreach (var nameItem in names)
        {
            ScoreDatas.Add(new ItemScoreData
            {
                itemName = nameItem,
                score =  1,
            });
        }
    }
}
