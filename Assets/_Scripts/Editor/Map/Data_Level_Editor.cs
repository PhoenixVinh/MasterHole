using _Scripts.Editor.Map;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(LevelGamePlaySO))]
[CanEditMultipleObjects]
public class Data_Level_Editor: Editor
{
    public SerializedProperty levelSpawnData;

     private void OnEnable()
     {
         levelSpawnData = serializedObject.FindProperty("levelSpawnData");
     }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Create Mission Prefab", GUILayout.Height(30), GUILayout.Width(240)))
        {
           
            LevelGamePlaySO levelGamePlaySo = target as LevelGamePlaySO;
            CreateMissionPrefab.ShowWindow(levelGamePlaySo);
        }
        GUILayout.FlexibleSpace();
    }
}
