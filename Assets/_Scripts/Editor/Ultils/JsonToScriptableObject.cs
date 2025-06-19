//using UnityEngine;
//using UnityEditor;
//using System.IO;
//using System.Collections.Generic;

//public class JsonToScriptableObjectConverter : EditorWindow {
//    private TextAsset jsonFile; // File JSON (.txt hoặc .json) để chuyển đổi
//    private string outputAssetName = "NewItemSpawnListAsset"; // Tên mặc định cho asset ScriptableObject mới
//    private string outputPath = "Assets/Data"; // Đường dẫn mặc định để lưu asset ScriptableObject

//    [MenuItem("Tools/JSON to ScriptableObject Converter")]
//    public static void ShowWindow() {
//        GetWindow<JsonToScriptableObjectConverter>("JSON to SO Converter");
//    }

//    private void OnGUI() {
//        GUILayout.Label("Chuyển đổi JSON sang ScriptableObject", EditorStyles.boldLabel);

//        // Trường để chọn file JSON
//        jsonFile = (TextAsset)EditorGUILayout.ObjectField(
//            "File JSON (.txt/.json):",
//            jsonFile,
//            typeof(TextAsset),
//            false // false vì bạn không muốn nó là một asset trong scene
//        );

//        // Trường nhập tên asset đầu ra
//        outputAssetName = EditorGUILayout.TextField("Tên Asset ScriptableObject:", outputAssetName);

//        // Trường nhập đường dẫn lưu và nút Browse
//        EditorGUILayout.BeginHorizontal();
//        outputPath = EditorGUILayout.TextField("Đường dẫn lưu:", outputPath);
//        if (GUILayout.Button("Browse", GUILayout.Width(60))) {
//            string selectedPath = EditorUtility.OpenFolderPanel("Chọn thư mục lưu ScriptableObject", outputPath, "");
//            if (!string.IsNullOrEmpty(selectedPath)) {
//                // Chuyển đổi đường dẫn tuyệt đối sang đường dẫn tương đối trong Unity
//                if (selectedPath.StartsWith(Application.dataPath)) {
//                    outputPath = "Assets" + selectedPath.Substring(Application.dataPath.Length);
//                } else {
//                    EditorUtility.DisplayDialog("Lỗi", "Vui lòng chọn một thư mục bên trong thư mục Assets của dự án.", "OK");
//                }
//            }
//        }
//        EditorGUILayout.EndHorizontal();


//        if (GUILayout.Button("Chuyển đổi và Lưu")) {
//            ConvertJsonToScriptableObject();
//        }
//    }

//    private void ConvertJsonToScriptableObject() {
//        if (jsonFile == null) {
//            EditorUtility.DisplayDialog("Lỗi", "Vui lòng kéo và thả file JSON vào trường 'File JSON'.", "OK");
//            return;
//        }

//        if (string.IsNullOrEmpty(outputAssetName)) {
//            EditorUtility.DisplayDialog("Lỗi", "Tên Asset không được để trống.", "OK");
//            return;
//        }

//        // Kiểm tra đường dẫn lưu hợp lệ và tồn tại
//        if (string.IsNullOrEmpty(outputPath) || !AssetDatabase.IsValidFolder(outputPath)) {
//            EditorUtility.DisplayDialog("Lỗi", "Đường dẫn lưu không hợp lệ. Vui lòng chọn một thư mục hợp lệ trong Assets.", "OK");
//            return;
//        }

//        string jsonString = jsonFile.text;

//        try {
//            // Sử dụng lớp wrapper để deserialize JSON
//            ItemSpawnData newItemSpawnListSO = JsonUtility.FromJson<ItemSpawnData>(jsonString);

            

//            // Tạo một instance mới của ScriptableObject
          

//            // Tạo đường dẫn đầy đủ cho asset mới
//            string fullPath = Path.Combine(outputPath, outputAssetName + ".asset");

//            // Tạo asset trong Project database
//            AssetDatabase.CreateAsset(newItemSpawnListSO, fullPath);
//            AssetDatabase.SaveAssets(); // Lưu tất cả các asset đang chờ thay đổi
//            AssetDatabase.Refresh(); // Làm mới Project window để hiển thị asset mới

//            EditorUtility.DisplayDialog("Thành công", $"Đã tạo ScriptableObject '{outputAssetName}.asset' tại '{fullPath}'", "OK");
//            Debug.Log($"Đã chuyển đổi JSON sang ScriptableObject thành công tại: {fullPath}");

//            // Chọn asset mới tạo trong Project window để người dùng dễ nhìn thấy
//            EditorGUIUtility.PingObject(newItemSpawnListSO);
//            Selection.activeObject = newItemSpawnListSO;

//        } catch (System.ArgumentException ae) {
//            // JsonUtility.FromJson ném ArgumentException nếu JSON bị định dạng sai hoặc không khớp kiểu
//            EditorUtility.DisplayDialog("Lỗi JSON", $"Lỗi định dạng JSON hoặc không khớp kiểu dữ liệu: {ae.Message}\nVui lòng đảm bảo JSON của bạn khớp với cấu trúc ItemSpawnListWrapper.", "OK");
//            Debug.LogError($"Lỗi Deserialization JSON: {ae.Message}");
//        } catch (System.Exception e) {
//            EditorUtility.DisplayDialog("Lỗi", $"Đã xảy ra lỗi khi chuyển đổi: {e.Message}", "OK");
//            Debug.LogError($"Lỗi chuyển đổi: {e.Message}");
//        }
//    }
//}