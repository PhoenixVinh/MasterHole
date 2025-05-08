//
//
// using UnityEngine;
// using UnityEditor;
// using System.IO;
//
// using _Scripts.Data;
//
// [CustomEditor(typeof(LevelSO))]
// public class LevelSOEditor : Editor
// {
//     private LevelSO levelSO;
//     private string saveDirectory = "Assets/_Data/LevelSO"; // Default save directory
//     private bool showDirectoryField = false;
//
//     private void OnEnable()
//     {
//         levelSO = (LevelSO)target;
//
//         // Try to find an existing "Levels" directory in Assets/Data
//         if (Directory.Exists(Application.dataPath + "/_Data/LevelSO"))
//         {
//             saveDirectory = "Assets/_Data/LevelSO";
//         }
//         else
//         {
//             // If not found, default to Assets and show the directory field
//             saveDirectory = "Assets";
//             showDirectoryField = true;
//         }
//     }
//
//     public override void OnInspectorGUI()
//     {
//         serializedObject.Update();
//
//         EditorGUILayout.PropertyField(serializedObject.FindProperty("LevelID"));
//         EditorGUILayout.PropertyField(serializedObject.FindProperty("AmountExp"));
//         EditorGUILayout.PropertyField(serializedObject.FindProperty("Radious"));
//
//         if (showDirectoryField)
//         {
//             EditorGUILayout.BeginHorizontal();
//             EditorGUILayout.LabelField("Save Directory:", GUILayout.Width(EditorGUIUtility.labelWidth));
//             saveDirectory = EditorGUILayout.TextField(saveDirectory);
//             if (GUILayout.Button("Browse", GUILayout.Width(80)))
//             {
//                 string newPath = EditorUtility.OpenFolderPanel("Select Save Directory", saveDirectory, "");
//                 if (!string.IsNullOrEmpty(newPath))
//                 {
//                     if (newPath.StartsWith(Application.dataPath))
//                     {
//                         saveDirectory = "Assets" + newPath.Substring(Application.dataPath.Length);
//                     }
//                     else
//                     {
//                         Debug.LogError("Selected directory must be within the project's Assets folder.");
//                     }
//                 }
//             }
//             EditorGUILayout.EndHorizontal();
//         }
//
//         if (GUILayout.Button("Save Data by Index"))
//         {
//             SaveDataByIndex();
//         }
//
//         serializedObject.ApplyModifiedProperties();
//     }
//
//     private void SaveDataByIndex()
//     {
//         if (string.IsNullOrEmpty(saveDirectory))
//         {
//             EditorUtility.DisplayDialog("Error", "Save directory is not set.", "OK");
//             return;
//         }
//
//         string fullDirectoryPath = Application.dataPath + saveDirectory.Substring("Assets".Length);
//
//         if (!Directory.Exists(fullDirectoryPath))
//         {
//             Directory.CreateDirectory(fullDirectoryPath);
//         }
//
//         // Get all existing LevelSO files in the directory
//         string[] existingFiles = Directory.GetFiles(fullDirectoryPath, "Level_*.asset");
//         int nextIndex = existingFiles.Length + 2;
//         string savePath = Path.Combine(saveDirectory, $"Level_{nextIndex:D3}.asset"); // Format with leading zeros
//
//         AssetDatabase.CreateAsset(levelSO, savePath);
//         AssetDatabase.SaveAssets();
//         AssetDatabase.Refresh();
//
//         //Debug.Log($"Level data saved to: {savePath}");
//     }
// }