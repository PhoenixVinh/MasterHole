using System;
using System.Collections.Generic;
using System.IO;
using Unity.Plastic.Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace _Scripts.Editor.Map
{
    public class MapEditor : EditorWindow
    { 
        private string jsonFilePath = "Assets/EGL_endless8_1 Level Json.txt";
        private List<InstantiateData> instantiateDataList = new List<InstantiateData>();
        private List<SpawnerData> spawnerDataList = new List<SpawnerData>();
        private Vector2 scrollPosition;

        [MenuItem("Tools/JSON to Class Converter")]
        public static void ShowWindow()
        {
            GetWindow<MapEditor>("JSON to Class Converter");
        }

        private void OnGUI()
        {
            GUILayout.Label("JSON to Class Converter", EditorStyles.boldLabel);

            // Input field for JSON file path
            GUILayout.Label("JSON File Path:");
            jsonFilePath = EditorGUILayout.TextField(jsonFilePath);

            // Button to load and convert JSON
            if (GUILayout.Button("Load and Convert JSON"))
            {
                ConvertJsonToClasses();
            }

            // Display results
            if (instantiateDataList.Count > 0 || spawnerDataList.Count > 0)
            {
                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
                
                GUILayout.Label("InstantiateData:", EditorStyles.boldLabel);
                string[] datalabels = new string[instantiateDataList.Count];
                int index = 0;
                foreach (var data in instantiateDataList)
                {
                    // Use TextField to allow copying ItemID
                    GUILayout.Label("Item ID:");
                    EditorGUILayout.TextField(data.itemId ?? "Null");
                    datalabels[index++] = data.itemId ?? "Null"; 
                    
                    GUILayout.Label($"Item Data Count: {data.itemData?.Length ?? 0}");
                }

                GUILayout.Label("SpawnerData:", EditorStyles.boldLabel);
                
                foreach (var data in spawnerDataList)
                {
                    GUILayout.Label($"Prefab ID: {data.pfbId}");
                    GUILayout.Label($"Position: ({data.Pos.x}, {data.Pos.y}, {data.Pos.z})");
                    GUILayout.Label($"Size: ({data.Size.x}, {data.Size.y}, {data.Size.z})");
                    GUILayout.Label($"Rotation: ({data.Rot.x}, {data.Rot.y}, {data.Rot.z})");
                }
                string filePath = "Assets/TextRead.txt";
                try
                {
                    // Use StreamWriter to write the list of strings to the specified file.
                    // The 'using' statement ensures that the StreamWriter is properly disposed
                    // after it's used, even if an exception occurs.
                    using (StreamWriter writer = new StreamWriter(filePath))
                    {
                        // Iterate through each string in the list.
                        foreach (string line in datalabels)
                        {
                            // Write the current string to the file, followed by a newline character.
                            writer.WriteLine(line);
                        }
                    }

                    // Optional: Print a success message to the console.
                  
                }
                catch (Exception ex)
                {
                    // If any error occurs during the file writing process, catch the exception
                    // and print an error message to the console. You might want to handle
                    // different types of exceptions more specifically in a production environment.
                    Debug.Log($"Error saving to file: {ex.Message}");
                }

                // Button to save results
                if (GUILayout.Button("Save to JSON"))
                {
                    SaveToJson();
                }

                EditorGUILayout.EndScrollView();
            }
        }

        private void ConvertJsonToClasses()
        {
            try
            {
                if (!File.Exists(jsonFilePath))
                {
                    Debug.LogError($"JSON file not found at: {jsonFilePath}");
                    return;
                }

                // Read JSON file
                string jsonContent = File.ReadAllText(jsonFilePath);

                // Deserialize JSON to LevelConfig
                LevelConfig levelConfig = JsonConvert.DeserializeObject<LevelConfig>(jsonContent);

                if (levelConfig == null)
                {
                    Debug.LogError("Failed to deserialize JSON.");
                    return;
                }

                // Assign InstantiateData
                instantiateDataList.Clear();
                if (levelConfig.InstantiateData != null)
                {
                    instantiateDataList.AddRange(levelConfig.InstantiateData);
                }

                // Convert GridConfig to SpawnerData
                spawnerDataList.Clear();
                if (levelConfig.SpawnerData != null)
                {
                    foreach (var grid in levelConfig.SpawnerData)
                    {
                        var spawner = new SpawnerData
                        {
                            Pos = grid.Pos,
                            Size = grid.Size,
                            Rot = grid.Rot,
                            pfbId = grid.pfbId
                        };
                        spawnerDataList.Add(spawner);
                    }
                }

                Debug.Log($"Conversion successful! Loaded {instantiateDataList.Count} InstantiateData and {spawnerDataList.Count} SpawnerData entries.");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Error converting JSON: {ex.Message}");
            }
        }

        private void SaveToJson()
        {
            try
            {
                // Create output object
                var output = new
                {
                    InstantiateData = instantiateDataList,
                    SpawnerData = spawnerDataList
                    
                };

                // Serialize to JSON
                string outputJson = JsonConvert.SerializeObject(output, Formatting.Indented);
                string nameOutput = GetFileNameFromPath(jsonFilePath);
                // Save to file
                string outputPath = Path.Combine(Application.dataPath, $"Resources/JsonData/{nameOutput}.json");
                File.WriteAllText(outputPath, outputJson);

                Debug.Log($"Saved converted data to: {outputPath}");
                AssetDatabase.Refresh();
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Error saving JSON: {ex.Message}");
            }
        }
        
        
        
        public static string GetFileNameFromPath(string path)
        {
            path = path.Replace(".txt", "");
            if (string.IsNullOrEmpty(path))
            {
                return string.Empty;
            }

            int lastSlashIndex = path.LastIndexOf('/');
            if (lastSlashIndex >= 0)
            {
                return path.Substring(lastSlashIndex + 1);
            }
            else
            {
                // Trường hợp không có dấu "/", trả về toàn bộ chuỗi
                return path;
            }
        }
    }
}