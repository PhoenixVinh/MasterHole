using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System;

public class LevelSpawnDataConverterWindow : EditorWindow
{
    private TextAsset yamlAsset;

    [MenuItem("Window/Level Spawn Data Converter")]
    public static void ShowWindow()
    {
        GetWindow<LevelSpawnDataConverterWindow>("Level Spawn Data Converter");
    }

    private void OnGUI()
    {
        GUILayout.Label("Convert YAML to ScriptableObject", EditorStyles.boldLabel);

        yamlAsset = (TextAsset)EditorGUILayout.ObjectField("YAML Asset", yamlAsset, typeof(TextAsset), false);

        if (GUILayout.Button("Convert"))
        {
            if (yamlAsset == null)
            {
                EditorUtility.DisplayDialog("Error", "Please assign a YAML asset.", "OK");
                return;
            }

            ConvertToScriptableObject();
        }
    }

    private void ConvertToScriptableObject()
    {
        try
        {
            // Parse the YAML manually
            var yamlData = ParseYamlManually(yamlAsset.text);

            // Create a new ScriptableObject
            LevelSpawnData levelSpawnData = ScriptableObject.CreateInstance<LevelSpawnData>();
            levelSpawnData.listItemSpawns = new List<ItemSpawn>();

            // Extract listItemSpawns from parsed data
            var listItemSpawns = yamlData["MonoBehaviour"]as List<object>;
            foreach (var item in listItemSpawns)
            {
                var itemDict = item as Dictionary<string, object>;
                var itemSpawn = new ItemSpawn
                {
                    id = itemDict["id"] as string,
                    listSpawnDatas = new List<ItemSpawnData>()
                };

                var spawnDatas = itemDict["listSpawnDatas"] as List<object>;
                // foreach (var spawn in spawnDatas)
                // {
                //     var spawnDict = spawn as Dictionary<string, object>;
                //     var spawnData = new ItemSpawnData()
                //     {
                //         p = (Vector3)spawnDict["p"],
                //         r = (Vector3)spawnDict["r"],
                //         s = (Vector3)spawnDict["s"],
                //         kinematic = (bool)spawnDict["kinematic"]
                //     };
                //     itemSpawn.listSpawnDatas.Add(spawnData);
                // }

                levelSpawnData.listItemSpawns.Add(itemSpawn);
            }

            // Save the ScriptableObject as an asset
            string outputPath = EditorUtility.SaveFilePanelInProject(
                "Save ScriptableObject",
                "LevelSpawnData",
                "asset",
                "Choose where to save the ScriptableObject"
            );

            if (!string.IsNullOrEmpty(outputPath))
            {
                AssetDatabase.CreateAsset(levelSpawnData, outputPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                EditorUtility.DisplayDialog("Success", "ScriptableObject created successfully!", "OK");
            }
        }
        catch (Exception ex)
        {
            EditorUtility.DisplayDialog("Error", $"Failed to convert: {ex.Message}", "OK");
            Debug.LogError(ex);
        }
    }

    private Dictionary<string, object> ParseYamlManually(string yamlContent)
    {
        var result = new Dictionary<string, object>
        {
            ["MonoBehaviour"] = new Dictionary<string, object>
            {
                ["listItemSpawns"] = new List<object>()
            }
        };
        var itemSpawns = result["MonoBehaviour"] as List<object>;

        string[] lines = yamlContent.Split(new[] { "\n" }, StringSplitOptions.None);
        ItemSpawn currentItemSpawn = null;
        ItemSpawnData currentSpawnData = null;
        Dictionary<string, float> currentVector = null;
        string currentVectorKey = null;

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i].Trim();

            // Skip empty lines, comments, or YAML headers
            if (string.IsNullOrEmpty(line) || line.StartsWith("%") || line.StartsWith("---") || line.StartsWith("!")) continue;

            // Start of a new item spawn
            if (line.StartsWith("- id:"))
            {
                currentItemSpawn = new ItemSpawn
                {
                    id = line.Replace("- id:", "").Trim(),
                    listSpawnDatas = new List<ItemSpawnData>()
                };
                itemSpawns.Add(new Dictionary<string, object>
                {
                    ["id"] = currentItemSpawn.id,
                    ["listSpawnDatas"] = currentItemSpawn.listSpawnDatas
                });
                continue;
            }

            // Start of a new spawn data entry
            if (line == "- p:" && currentItemSpawn != null)
            {
                currentSpawnData = new ItemSpawnData();
                currentItemSpawn.listSpawnDatas.Add(currentSpawnData);
                continue;
            }

            // Vector fields (p, r, s)
            if ((line == "p:" || line == "r:" || line == "s:") && currentSpawnData != null)
            {
                currentVectorKey = line.TrimEnd(':');
                currentVector = new Dictionary<string, float>();
                continue;
            }

            // Vector components (x, y, z)
            if (currentVector != null && (line.StartsWith("x:") || line.StartsWith("y:") || line.StartsWith("z:")))
            {
                var parts = line.Split(':');
                if (parts.Length == 2)
                {
                    string value = parts[1].Trim();
                    if (float.TryParse(value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float floatValue))
                    {
                        currentVector[parts[0].Trim()] = floatValue;
                    }
                    else
                    {
                        throw new Exception($"Failed to parse float value: {value} at line {i + 1}");
                    }
                }

                // When vector is complete (x, y, z parsed)
                if (currentVector.Count == 3)
                {
                    Vector3 vector = new Vector3(currentVector["x"], currentVector["y"], currentVector["z"]);
                    if (currentVectorKey == "p") currentSpawnData.p = new Position(vector);
                    else if (currentVectorKey == "r") currentSpawnData.r =  new Rotation(vector);
                    else if (currentVectorKey == "s") currentSpawnData.s = new Scale(vector);
                    currentVector = null;
                    currentVectorKey = null;
                }
                continue;
            }

            // Kinematic field
            if (line.StartsWith("kinematic:") && currentSpawnData != null)
            {
                string value = line.Replace("kinematic:", "").Trim();
                currentSpawnData.kinematic = value == "1";
                currentSpawnData = null; // Reset after completing a spawn data entry
                continue;
            }
        }

        return result;
    }
}