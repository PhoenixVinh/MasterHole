using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

public class MissionSOChecker : EditorWindow
{
    private static string specificFolderPath = "Assets/_Data/MissionNewSO"; // Thay đổi đường dẫn này tới thư mục bạn muốn kiểm tra
    private static string specificFolderPathImage = "Assets/DataModelNew_01/Texture2D";
    [MenuItem("Tools/Check MissionSO Images in Specific Folder")]
    public static void CheckMissionSOImagesInFolder()
    {
        // Đảm bảo đường dẫn thư mục hợp lệ
        if (!Directory.Exists(specificFolderPath))
        {
            Debug.LogError($"Thư mục {specificFolderPath} không tồn tại!");
            return;
        }

        // Tìm tất cả các file MissionSO trong thư mục cụ thể
        string[] guids = AssetDatabase.FindAssets("t:MissionSO", new[] { specificFolderPath });

        bool hasNullImage = false;
        string result = "";

        List<String> nameAssets = new List<string>();
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            MissionSO missionSO = AssetDatabase.LoadAssetAtPath<MissionSO>(path);

            if (missionSO != null && missionSO.misstionsData != null)
            {
                // Kiểm tra mỗi MissionData trong danh sách
                foreach (MissionData missionData in missionSO.misstionsData)
                {
                    if (missionData.image != null) continue;
                    string[] imageGuids = AssetDatabase.FindAssets($"t:Sprite", new[] { specificFolderPathImage });
                    bool found = false;
                    foreach (string imageGuid in imageGuids)
                    {
                        string imagePath = AssetDatabase.GUIDToAssetPath(imageGuid);
                        Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(imagePath);
                        if (sprite != null && sprite.name == missionData.idItem)
                        {
                            missionData.image = sprite;
                            EditorUtility.SetDirty(missionSO);
                            Debug.Log($"Assigned image '{sprite.name}' to mission '{missionData.idItem}' in '{missionSO.name}'");
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        Debug.LogWarning($"No sprite found with exact name '{missionData.idItem}' in folder {specificFolderPathImage}");
                    }
                    AssetDatabase.SaveAssets();
                }
            }
        }

        foreach (var item in nameAssets)
        {
            Debug.Log($"{item}");
        }
        if (!hasNullImage)
        {
            Debug.Log("Không tìm thấy image null trong các file MissionSO trong thư mục được chỉ định.");
        }

        Debug.Log($"Kiểm tra image MissionSO trong thư mục {specificFolderPath} đã hoàn tất!");
    }
}