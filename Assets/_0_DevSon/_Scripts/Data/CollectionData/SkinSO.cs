
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SkinData
{
    public int levelUnlock;
    public Sprite  image;
}

[CreateAssetMenu(fileName = "SkinSO", menuName = "Data/Skin Data")]
public class SkinSO : ScriptableObject
{
    public List<SkinData>  skins;
}