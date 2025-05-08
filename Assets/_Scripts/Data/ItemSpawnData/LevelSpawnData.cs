using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "LevelSpawnData", menuName = "LevelSpawnData")]
public class LevelSpawnData : ScriptableObject
{
  
    public List<ItemSpawn> listItemSpawns; // Matches listItemSpawns in YAML
}