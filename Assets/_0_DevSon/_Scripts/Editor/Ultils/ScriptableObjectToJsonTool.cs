using UnityEngine;
using UnityEditor;
using System.IO; // Để làm việc với file
using System.Collections.Generic; // Để sử dụng List

public class ScriptableObjectToJsonTool : EditorWindow {
    private ScriptableObject targetScriptableObject; // ScriptableObject mà chúng ta muốn chuyển đổi
    private string outputFileName = "output_data.json";

    [MenuItem("Tools/ScriptableObject to JSON Converter")]
    public static void ShowWindow() {
        GetWindow<ScriptableObjectToJsonTool>("SO to JSON Converter");
    }

    private void OnGUI() {
        GUILayout.Label("Chuyển đổi ScriptableObject sang JSON", EditorStyles.boldLabel);

        // Trường để người dùng kéo thả ScriptableObject vào
        targetScriptableObject = (ScriptableObject)EditorGUILayout.ObjectField(
            "ScriptableObject:",
            targetScriptableObject,
            typeof(ScriptableObject),
            false
        );

        // Trường nhập tên file đầu ra
        outputFileName = EditorGUILayout.TextField("Tên file JSON:", outputFileName);

        if (GUILayout.Button("Lưu sang JSON")) {
            if (targetScriptableObject == null) {
                EditorUtility.DisplayDialog("Lỗi", "Vui lòng chọn một ScriptableObject để chuyển đổi.", "OK");
                return;
            }

            if (string.IsNullOrEmpty(outputFileName)) {
                EditorUtility.DisplayDialog("Lỗi", "Tên file JSON không được để trống.", "OK");
                return;
            }

            ConvertAndSaveToJson(targetScriptableObject, outputFileName);
        }
    }

    private void ConvertAndSaveToJson(ScriptableObject so, string fileName) {
        // Sử dụng JsonUtility.ToJson để chuyển đổi ScriptableObject thành chuỗi JSON
        // Tham số 'true' để làm cho JSON dễ đọc hơn với định dạng đẹp (pretty print)
        string jsonString = JsonUtility.ToJson(so, true);

        // Xác định đường dẫn lưu file
        // Application.dataPath là thư mục Assets của dự án
        string outputPath = Path.Combine(Application.dataPath, fileName);

        try {
            File.WriteAllText(outputPath, jsonString);
            EditorUtility.DisplayDialog("Thành công", $"Đã lưu dữ liệu từ '{so.name}' vào '{outputPath}'", "OK");
            Debug.Log($"ScriptableObject '{so.name}' đã được lưu thành JSON tại: {outputPath}");

            // Làm mới Project window để hiển thị file JSON mới tạo
            AssetDatabase.Refresh();
        } catch (System.Exception e) {
            EditorUtility.DisplayDialog("Lỗi", $"Không thể lưu file JSON: {e.Message}", "OK");
            Debug.LogError($"Lỗi khi lưu JSON: {e.Message}");
        }
    }
}