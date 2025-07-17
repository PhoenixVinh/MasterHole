
using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
[CreateAssetMenu(fileName = "MapSO", menuName = "Data/MapSO", order = 1)]
public class MapSO : ScriptableObject
{
    public int width;
    public int height;
    public string mapData;

    
    public Vector3 positionMap;
    public Vector3 rotationMap = new Vector3(0, 180, 0);
    
}