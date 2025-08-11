using UnityEngine;
using UnityEditor;
using System.IO;

public class ConvertJsonToSOMap : EditorWindow
{
    private string jsonFilePath = "";
    private string saveFolderPath = "Assets/";
    private string scriptableObjectTypeName = "YourScriptableObject"; // Change to your SO class name

    [MenuItem("Tools/Json Map Converter")]
    public static void ShowWindow()
    {
        GetWindow<ConvertJsonToSOMap>("JSON To SO Converter MAP");
    }

    void OnGUI()
    {
        GUILayout.Label("Convert JSON File To ScriptableObject", EditorStyles.boldLabel);

        jsonFilePath = EditorGUILayout.TextField("JSON File Path", jsonFilePath);
        GUILayout.Label("JSON File: " + jsonFilePath);

        saveFolderPath = EditorGUILayout.TextField("Save Folder", saveFolderPath);
        scriptableObjectTypeName = EditorGUILayout.TextField("SO Type Name", scriptableObjectTypeName);

        if (GUILayout.Button("Convert"))
        {
            ConvertJsonToSO();
        }
    }

    void ConvertJsonToSO()
    {
        
        string[] jsonFiles = Directory.GetFiles(Path.GetDirectoryName(jsonFilePath), "*.json");
        
        foreach (string file in jsonFiles)
        {
            if (file.EndsWith(".json"))
            {
                string json = File.ReadAllText(file);
                ScriptableObject so = ScriptableObject.CreateInstance<MapSO>(); // Change to your specific SO type
                JsonUtility.FromJsonOverwrite(json, so);

                string fileName = Path.GetFileNameWithoutExtension(file) + ".asset";
                string assetPath = Path.Combine(saveFolderPath, fileName);

                AssetDatabase.CreateAsset(so, assetPath);
                AssetDatabase.SaveAssets();

                Debug.Log("ScriptableObject created at: " + assetPath);
            }
        }

       
    }
   
}