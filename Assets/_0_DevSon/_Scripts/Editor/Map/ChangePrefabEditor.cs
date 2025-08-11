using System;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Object = UnityEngine.Object;

public class PrefabMoverEditorWindow : EditorWindow
{
    private string inputFilePath = "";
    private string inputPrefabPath = "Assets/"; // Default to the Assets folder
    private string outputPath = "Assets/Output/"; // Default output path
    private bool movePrefabs = false; // Option to move instead of copy
    private Vector2 scrollPosition;

    [MenuItem("Window/Prefab Mover")]
    public static void ShowWindow()
    {
        GetWindow<PrefabMoverEditorWindow>("Prefab Mover");
    }

    private void OnGUI()
    {
        // Use a ScrollView in case the window content exceeds the window size.
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        GUILayout.Label("Settings", EditorStyles.boldLabel);

        // Input file path - where the list of prefab names is.
        inputFilePath = EditorGUILayout.TextField("Input File Path (.txt)", inputFilePath);
        // Input prefab path - where to search for the prefabs.
        inputPrefabPath = EditorGUILayout.TextField("Input Prefab Path", inputPrefabPath);
        // Output path - where to put the found prefabs.
        outputPath = EditorGUILayout.TextField("Output Path", outputPath);
        // Toggle to move or copy.
        movePrefabs = EditorGUILayout.Toggle("Move Prefabs", movePrefabs);

        // Ensure the output path exists.
        if (!Directory.Exists(outputPath))
        {
            EditorGUILayout.HelpBox("Output path does not exist!  It will be created.", MessageType.Warning);
            if (GUILayout.Button("Create Output Path"))
            {
                Directory.CreateDirectory(outputPath);
            }
        }

        // Button to process the prefabs.
        if (GUILayout.Button("Process Prefabs"))
        {
            ProcessPrefabs();
        }

        EditorGUILayout.EndScrollView();
    }

    private void ProcessPrefabs()
    {
        if (string.IsNullOrEmpty(inputFilePath))
        {
            EditorUtility.DisplayDialog("Error", "Please specify an input file path.", "OK");
            return;
        }

        if (!File.Exists(inputFilePath))
        {
            EditorUtility.DisplayDialog("Error", "Input file does not exist.", "OK");
            return;
        }

        if (string.IsNullOrEmpty(outputPath))
        {
            EditorUtility.DisplayDialog("Error", "Please specify an output path.", "OK");
            return;
        }

        if (!Directory.Exists(outputPath))
        {
             Directory.CreateDirectory(outputPath);
        }

        List<string> prefabNames;
        try
        {
            // Read the prefab names from the input file.  Use .NET's file reading.
            prefabNames = File.ReadAllLines(inputFilePath).ToList();
        }
        catch (Exception e)
        {
            EditorUtility.DisplayDialog("Error Reading File", "Error reading the input file: " + e.Message, "OK");
            return;
        }

        if (prefabNames.Count == 0)
        {
            EditorUtility.DisplayDialog("Warning", "Input file is empty.", "OK");
            return;
        }

        // Use a HashSet to track already processed file names to prevent duplicates.
        HashSet<string> processedFileNames = new HashSet<string>();
        int movedCount = 0;
        int copiedCount = 0;
        int notFoundCount = 0;
        int skippedCount = 0;

        foreach (string prefabName in prefabNames)
        {
            string trimmedPrefabName = prefabName.Trim(); // Remove leading/trailing spaces.  IMPORTANT
            if (string.IsNullOrEmpty(trimmedPrefabName)) continue; // Skip empty lines in the text file

            // Find the prefab.  Use Editor's AssetDatabase.FindAssets.  This searches the project.
            string[] guids = AssetDatabase.FindAssets(trimmedPrefabName, new[] { inputPrefabPath }); // Search within inputPrefabPath
            if (guids.Length == 0)
            {
                Debug.LogWarning("Prefab not found: " + trimmedPrefabName);
                notFoundCount++;
                continue;
            }
            if (guids.Length > 1)
            {
                Debug.LogWarning("Multiple prefabs found with name: " + trimmedPrefabName + ".  Using the first one.");
            }
            string prefabPath = AssetDatabase.GUIDToAssetPath(guids[0]);  // Get the path from the GUID.
            Object prefab = AssetDatabase.LoadAssetAtPath(prefabPath, typeof(GameObject)); // Load

            if (prefab == null)
            {
                Debug.LogWarning("Could not load prefab at path: " + prefabPath);
                notFoundCount++;
                continue;
            }

            string fileName = Path.GetFileName(prefabPath); // Get filename with extension
            string destFilePath = Path.Combine(outputPath, fileName);

            // Check for duplicates in the *destination* folder.
            if (processedFileNames.Contains(fileName))
            {
                Debug.LogWarning("Duplicate prefab name in destination: " + fileName + ". Skipping.");
                skippedCount++;
                continue;
            }

            processedFileNames.Add(fileName); // Add to the set so we don't process it again.

            if (movePrefabs)
            {
                // Use AssetDatabase.MoveAsset for moving.  Handles file system AND metafile.
                AssetDatabase.MoveAsset(prefabPath, destFilePath);
                if (AssetDatabase.MoveAsset(prefabPath, destFilePath) == "")
                {
                    Debug.Log("Moved prefab: " + fileName + " to " + destFilePath);
                    movedCount++;
                }
                else
                {
                    Debug.LogError("Failed to move prefab: " + fileName + " to " + destFilePath);
                }
            }
            else
            {
                // Use AssetDatabase.CopyAsset for copying.
                if (AssetDatabase.CopyAsset(prefabPath, destFilePath))
                {
                    Debug.Log("Copied prefab: " + fileName + " to " + destFilePath);
                    copiedCount++;
                }
                else
                {
                    Debug.LogError("Failed to copy prefab: " + fileName + " to " + destFilePath);
                }
            }
        }

        // Refresh the Asset Database to show the new files.
        AssetDatabase.Refresh();

        // Show a summary.
        string message = $"Processed {prefabNames.Count} prefabs.\n" +
                         $"Moved: {movedCount}\n" +
                         $"Copied: {copiedCount}\n" +
                         $"Not Found: {notFoundCount}\n" +
                         $"Skipped Duplicates: {skippedCount}";
        EditorUtility.DisplayDialog("Summary", message, "OK");
    }
}

