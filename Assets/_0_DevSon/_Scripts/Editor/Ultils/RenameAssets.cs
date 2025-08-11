using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class RenameAssets
{
    [MenuItem("Tools/Rename Assets From GUID Map")]
    public static void RenameAssetsFromMap()
    {
        // Đường dẫn đến file Guid_Map.txt (đặt trong thư mục Assets hoặc chỉ định đường dẫn tuyệt đối)
        string filePath = Path.Combine(Application.dataPath, "Guid_Map.txt");
        
        // Kiểm tra xem file tồn tại
        if (!File.Exists(filePath))
        {
            Debug.LogError($"File not found: {filePath}");
            return;
        }

        // Dictionary để lưu ánh xạ tên và GUID
        Dictionary<string, string> guidMap = new Dictionary<string, string>();
        
        try
        {
            // Đọc file Guid_Map.txt
            string[] lines = File.ReadAllLines(filePath, System.Text.Encoding.UTF8);
            foreach (string line in lines)
            {
                // Bỏ qua dòng trống
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                // Tách dòng thành tên và GUID
                string[] parts = line.Split(',');
                if (parts.Length != 2)
                {
                    Debug.LogWarning($"Invalid line format: {line}");
                    continue;
                }

                string name = parts[0].Trim();
                string guid = parts[1].Trim();

                // Kiểm tra GUID hợp lệ (khác "None" và đúng định dạng 32 ký tự hexa)
                if (guid == "None" || !IsValidGuid(guid))
                {
                    Debug.LogWarning($"Invalid or missing GUID for name: {name} (GUID: {guid})");
                    continue;
                }

                // Lưu vào dictionary
                if (!guidMap.ContainsKey(name))
                {
                    guidMap[name] = guid;
                }
                else
                {
                    Debug.LogWarning($"Duplicate name found: {name}");
                }
            }

            // Đổi tên asset
            int renamedCount = 0;
            foreach (var pair in guidMap)
            {
                string name = pair.Key;
                string guid = pair.Value;

                // Tìm đường dẫn asset từ GUID
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                if (string.IsNullOrEmpty(assetPath))
                {
                    Debug.LogWarning($"No asset found for GUID: {guid} (intended name: {name})");
                    continue;
                }

                // Lấy thư mục và phần mở rộng của asset
                string directory = Path.GetDirectoryName(assetPath);
                string extension = Path.GetExtension(assetPath);
                string newAssetPath = Path.Combine(directory, name + extension);

                // Kiểm tra xem tên mới đã tồn tại chưa
                if (File.Exists(newAssetPath) && newAssetPath != assetPath)
                {
                    Debug.LogWarning($"Cannot rename asset at {assetPath} to {name}{extension}: Name already exists.");
                    continue;
                }

                // Đổi tên asset
                string error = AssetDatabase.RenameAsset(assetPath, name);
                if (string.IsNullOrEmpty(error))
                {
                    Debug.Log($"Renamed asset at {assetPath} to {name}{extension}");
                    renamedCount++;
                }
                else
                {
                    Debug.LogError($"Failed to rename asset at {assetPath} to {name}{extension}: {error}");
                }
            }

            // Lưu thay đổi
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"Renaming complete. {renamedCount} assets renamed successfully.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error processing file {filePath}: {e.Message}");
        }
    }

    private static bool IsValidGuid(string guid)
    {
        // Kiểm tra GUID có đúng định dạng 32 ký tự hexa
        return System.Text.RegularExpressions.Regex.IsMatch(guid, @"^[0-9a-fA-F]{32}$");
    }
}