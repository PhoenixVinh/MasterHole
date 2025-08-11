using UnityEngine;
using UnityEditor;
using System;

namespace MasterHole.Editor
{
    public class AssignMap : EditorWindow
    {

        public string levelPath;
        public string mapPath;


        [MenuItem("Tools/Map/Assign Map")]
        private static void ShowWindow()
        {
            var window = GetWindow<AssignMap>();
            window.titleContent = new GUIContent("Assign Map");
            window.Show();
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Map Assignment", EditorStyles.boldLabel);

            EditorGUILayout.Space();

            //levelPath = Lay
            levelPath = EditorGUILayout.TextField(levelPath);
            mapPath = EditorGUILayout.TextField(mapPath);

            if (GUILayout.Button("Assign Map"))
            {
                AssignMapProcess();
            }
            
            
            
            
        }

        private void AssignMapProcess()
        {

            for (int i = 1; i <= 100; i++)
            {
                LevelGamePlaySO level = AssetDatabase.LoadAssetAtPath<LevelGamePlaySO>("Assets/Resources/DataLevelNewFixSO" + $"/Data_Level_{i}.asset");
                MapSO map = AssetDatabase.LoadAssetAtPath<MapSO>("Assets/_Data/DataItemMap/TileMap" + $"/TileMap_{i}.asset");

                if (level != null && map != null)
                {
                    level.mapItem = map;

                    EditorUtility.SetDirty(level);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
               
                if (level == null)
                {
                    Debug.Log("Null Level");
                }
                if(map == null)
                {
                    Debug.Log("Null Map");
                }
            }

            
        }
    }
}