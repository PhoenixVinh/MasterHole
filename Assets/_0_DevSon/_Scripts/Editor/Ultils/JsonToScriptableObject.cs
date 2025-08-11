using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class JsonToScriptableObjectConverter : EditorWindow {
    private string inputFolderPath = "Assets/JSON_Input"; 
    private string outputFolderPath = "Assets/SO_Output"; 
    private TextAsset jsonFile; // File JSON (.txt hoặc .json) để chuyển đổi
    //private string outputAssetName = "NewItemSpawnListAsset"; // Tên mặc định cho asset ScriptableObject mới
    private string outputPath = "Assets/_Data/DataItemMap/DataMap2/"; // Đường dẫn mặc định để lưu asset ScriptableObject

    [MenuItem("Tools/JSON to ScriptableObject Converter")]
    public static void ShowWindow() {
        GetWindow<JsonToScriptableObjectConverter>("JSON to SO Converter");
    }

    private void OnGUI() {
        GUILayout.Label("Chuyển đổi JSON sang ScriptableObject", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        inputFolderPath = EditorGUILayout.TextField(inputFolderPath);
        if (GUILayout.Button("Chọn thư mục", GUILayout.Width(100)))
        {
            string selectedPath = EditorUtility.OpenFolderPanel("Chọn thư mục JSON", inputFolderPath, "");
            if (!string.IsNullOrEmpty(selectedPath))
            {
                inputFolderPath = "Assets" + selectedPath.Replace(Application.dataPath, "");
            }
        }
        EditorGUILayout.EndHorizontal();
        // Trường để chọn thư mục lưu ScriptableObject
        GUILayout.Label("Thư mục lưu ScriptableObject:");
        EditorGUILayout.BeginHorizontal();
        outputFolderPath = EditorGUILayout.TextField(outputFolderPath);
        if (GUILayout.Button("Chọn thư mục", GUILayout.Width(100)))
        {
            string selectedPath = EditorUtility.OpenFolderPanel("Chọn thư mục lưu SO", outputFolderPath, "");
            if (!string.IsNullOrEmpty(selectedPath))
            {
                outputFolderPath = "Assets" + selectedPath.Replace(Application.dataPath, "");
            }
        }
        EditorGUILayout.EndHorizontal();


        if (GUILayout.Button("Chuyển đổi và Lưu")) {
            ConvertJsonToScriptableObject();
        }
    }
    

    
    private void ConvertJsonToScriptableObject() {
        
        string[] jsonFiles = Directory.GetFiles(inputFolderPath, "*.json", SearchOption.AllDirectories);
        List<string> processedFiles = new List<string>();
        int count = 1;
        
        foreach (string jsonFile in jsonFiles)
        {
            // Đọc nội dung JSON
            string jsonStringS = File.ReadAllText(jsonFile);
            string name = jsonFile.Substring(jsonFile.LastIndexOf("_") + 1,
                jsonFile.LastIndexOf(".") - jsonFile.LastIndexOf("_") - 1);
          
            LevelSpawnDataS1 newItemSpawnListSO = JsonUtility.FromJson<LevelSpawnDataS1>(jsonStringS);

            //Debug.Log(newItemSpawnListSO.Count);
            LevelSpawnData dataSO = ScriptableObject.CreateInstance<LevelSpawnData>();

            dataSO.SetData(newItemSpawnListSO.listItemSpawns);

            //Debug.Log(dataSO.listItemSpawns.Count);
            // // Tạo một instance mới của ScriptableObject
            //
            //
            // // Tạo đường dẫn đầy đủ cho asset mới
            string fullPath = Path.Combine(outputPath, "DataNew_" + name + ".asset");

            // Tạo asset trong Project database
            AssetDatabase.CreateAsset(dataSO, fullPath);
            AssetDatabase.SaveAssets(); // Lưu tất cả các asset đang chờ thay đổi
            AssetDatabase.Refresh(); // Làm mới Project window để hiển thị asset mới
            count++;
            Debug.Log("Chuyen doi thanh cong: " + fullPath);

        }


    }
}