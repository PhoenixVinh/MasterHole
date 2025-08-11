



[System.Serializable]
public class ItemData {
    public Vector3Convert pos;
    public Vector3Convert rot;
   
}

[System.Serializable]
public class InstantiateData
{
    public string itemId;
    public ItemData[] itemData;
}

[System.Serializable]
public class SpawnerData
{
    public Vector3Convert Pos;
    public Vector3Convert Size;
    public Vector3Convert Rot;
    public string pfbId;
}
[System.Serializable]
public class Vector3Convert
{
    public float x;
    public float y;
    public float z;
}

[System.Serializable]
public class LevelConfig
{
    public InstantiateData[] InstantiateData;
    public SpawnerData[] SpawnerData;
}
