using System;
using System.Collections.Generic;
using  UnityEngine;



[Serializable]
public class ItemSpawn
{
    public string id; // e.g., food_82
    public List<ItemSpawnData> listSpawnDatas; // List of spawn data
}

[Serializable]
public class ItemSpawnData
{
    public Position p;

    public Rotation r;

    public Scale s;

    public bool kinematic;

    public ItemData ToItemData(string id)
    {
        return null;
    }
   
}

[Serializable]
public class Position
{
    public float x;

    public float y;

    public float z;

    public Position()
    {
    }

    public Position(float x, float y, float z)
    {
    }

    public Position(Vector3 pos)
    {
        this.x = pos.x;
        this.y = pos.y;
        this.z = pos.z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(this.x, this.y, this.z);
    }
}
[Serializable]
public class Rotation
{
    public float x;

    public float y;

    public float z;

    public Rotation()
    {
    }

    public Rotation(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Rotation(Vector3 eulerAngles)
    {
    }

    public Vector3 ToVector3()
    {
        return new Vector3(this.x, this.y, this.z);
    }
}

[Serializable]
public class Scale
{
    public float x;

    public float y;

    public float z;

    public Scale()
    {
    }

    public Scale(float x, float y, float z)
    {
    }

    public Scale(Vector3 scale)
    {
        this.x = scale.x;
        this.y = scale.y;
        this.z = scale.z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(this.x, this.y, this.z);
    }
}