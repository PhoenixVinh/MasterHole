
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class savedPrefabs
{
    public string prefabName;

    public string categoryName;

    public float x;
    public float y;
    public float rotation;

}


[CreateAssetMenu(fileName = "MapSO", menuName = "Data/MapSO", order = 1)]
public class MapSO : ScriptableObject
{
    public float cellSize = 4f;
    public float gridWidth = 100f;
    public float gridHeight = 100f;
    public float offsetX = 0f;
    public float offsetY = 0f;
    public float offsetZ = 0f;
    public List<savedPrefabs> savedPrefabs;


}