using UnityEditor;
using UnityEngine;
using System.IO;

public class ChangeGUIDEditor : EditorWindow
{
    private string folderPath = "Assets"; // Default folder path
    private string oldGUID = "989a83c113073f4f1b8a2efc3331aa7e";
    private string newGUID = "textinput";
    private string statusMessage = "";

    [MenuItem("Tools/Change GUID Editor")]
    public static void ShowWindow()
    {
        GetWindow<ChangeGUIDEditor>("Change GUID Editor");
    }

    private void OnGUI()
    {
        GUILayout.Label("Change GUID in All .asset Files in Folder", EditorStyles.boldLabel);

        GUILayout.Space(10);

        GUILayout.Label("Folder Path (relative to Assets):");
        folderPath = EditorGUILayout.TextField(folderPath);

        GUILayout.Label("Old GUID:");
        oldGUID = EditorGUILayout.TextField(oldGUID);

        GUILayout.Label("New GUID:");
        newGUID = EditorGUILayout.TextField(newGUID);

        GUILayout.Space(10);

        if (GUILayout.Button("Change GUID in All Files"))
        {
            ChangeGUIDInFolder();
        }

        GUILayout.Space(10);
        GUILayout.Label(statusMessage, EditorStyles.wordWrappedLabel);
    }

    private void ChangeGUIDInFolder()
    {
        statusMessage = "";
        try
        {
            // Convert relative path to absolute path
            string absoluteFolderPath = Path.Combine(Application.dataPath, folderPath.Replace("Assets/", ""));
            if (!Directory.Exists(absoluteFolderPath))
            {
                statusMessage = "Error: Folder not found at " + folderPath;
                return;
            }

            // Get all .asset files in the folder
            string[] files = Directory.GetFiles(absoluteFolderPath, "*.asset", SearchOption.TopDirectoryOnly);
            if (files.Length == 0)
            {
                statusMessage = "Error: No .asset files found in " + folderPath;
                return;
            }

            int modifiedFiles = 0;
            foreach (string file in files)
            {
                // Read the file content
                string fileContent = File.ReadAllText(file);

                // Check if old GUID exists in the file
                if (!fileContent.Contains(oldGUID))
                {
                    statusMessage += $"Skipped {Path.GetFileName(file)}: Old GUID not found\n";
                    continue;
                }

                // Replace the GUID
                fileContent = fileContent.Replace(oldGUID, newGUID);

                // Write back to the file
                File.WriteAllText(file, fileContent);
                modifiedFiles++;

                statusMessage += $"Modified {Path.GetFileName(file)}: GUID changed to {newGUID}\n";
            }

            // Refresh Unity's asset database
            AssetDatabase.Refresh();

            if (modifiedFiles > 0)
            {
                statusMessage += $"Success: Modified {modifiedFiles} file(s) in {folderPath}";
            }
            else
            {
                statusMessage += "No files were modified.";
            }
        }
        catch (System.Exception ex)
        {
            statusMessage = "Error: " + ex.Message;
        }
    }
}