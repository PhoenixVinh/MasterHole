using UnityEngine;
using UnityEditor;
using UnityEditor.Formats.Fbx.Exporter;
using System.IO;
using System.Linq;

public class MeshToFBXConverter : EditorWindow
{
    private string inputDirectory = "Assets/Meshes"; // Default input directory
    private string outputDirectory = "Assets/ExportedMeshes"; // Default output directory

    [MenuItem("Tools/Convert Meshes to FBX by Directory")]
    public static void ShowWindow()
    {
        GetWindow<MeshToFBXConverter>("Meshes to FBX Converter");
    }

    private void OnGUI()
    {
        GUILayout.Label("Batch Mesh to FBX Converter (by Directory)", EditorStyles.boldLabel);

        // Input field for input directory
        inputDirectory = EditorGUILayout.TextField("Input Directory", inputDirectory);
        EditorGUILayout.HelpBox("Enter the directory containing .asset mesh files (e.g., Assets/Meshes).", MessageType.Info);

        // Input field for output directory
        outputDirectory = EditorGUILayout.TextField("Output Directory", outputDirectory);
        EditorGUILayout.HelpBox("Enter the directory where FBX files will be saved (e.g., Assets/ExportedMeshes).", MessageType.Info);

        if (GUILayout.Button("Convert All Meshes to FBX"))
        {
            ConvertMeshesToFBX();
        }
    }

    private void ConvertMeshesToFBX()
    {
        if (string.IsNullOrEmpty(inputDirectory))
        {
            Debug.LogError("Input directory is empty!");
            return;
        }

        if (string.IsNullOrEmpty(outputDirectory))
        {
            Debug.LogError("Output directory is empty!");
            return;
        }

        // Normalize input directory path
        inputDirectory = inputDirectory.Replace('\\', '/').TrimEnd('/');
        if (!Directory.Exists(inputDirectory))
        {
            Debug.LogError($"Input directory does not exist: {inputDirectory}");
            return;
        }

        // Ensure the output directory exists
        if (!Directory.Exists(outputDirectory))
        {
            Directory.CreateDirectory(outputDirectory);
            AssetDatabase.Refresh();
        }

        // Find all .asset files in the input directory
        string[] assetFiles = Directory.GetFiles(inputDirectory, "*.asset", SearchOption.TopDirectoryOnly);
        if (assetFiles.Length == 0)
        {
            Debug.LogWarning($"No .asset files found in directory: {inputDirectory}");
            return;
        }

        int successCount = 0;
        int failureCount = 0;

        // Process each .asset file
        foreach (string assetPath in assetFiles)
        {
            // Convert filesystem path to Unity asset path (relative to Assets)
            string unityAssetPath = assetPath.Replace(Application.dataPath, "Assets").Replace('\\', '/');
            Mesh mesh = AssetDatabase.LoadAssetAtPath<Mesh>(unityAssetPath);

            if (mesh != null)
            {
                string meshName = Path.GetFileNameWithoutExtension(assetPath);
                string outputPath = Path.Combine(outputDirectory, $"{meshName}.fbx");

                try
                {
                    // Create a temporary GameObject to hold the mesh
                    GameObject tempGameObject = new GameObject("TempMeshObject");
                    MeshFilter meshFilter = tempGameObject.AddComponent<MeshFilter>();
                    meshFilter.sharedMesh = mesh;
                    tempGameObject.AddComponent<MeshRenderer>(); // Optional, for compatibility

                    // Export the GameObject to FBX with default settings
                    ModelExporter.ExportObjects(outputPath, new Object[] { tempGameObject });

                    // Clean up
                    DestroyImmediate(tempGameObject);

                    Debug.Log($"Successfully exported FBX: {outputPath}");
                    successCount++;
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"Failed to export {meshName} to FBX: {ex.Message}");
                    failureCount++;
                }
            }
            else
            {
                Debug.LogWarning($"Skipping {Path.GetFileName(assetPath)}: Not a valid Mesh asset.");
                failureCount++;
            }
        }

        // Refresh AssetDatabase to show new FBX files
        AssetDatabase.Refresh();

        // Summary
        Debug.Log($"Batch conversion complete: {successCount} succeeded, {failureCount} failed.");
    }
}