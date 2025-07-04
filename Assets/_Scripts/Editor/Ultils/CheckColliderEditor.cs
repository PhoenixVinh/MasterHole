// using UnityEngine;
// using UnityEditor;
// using System.Linq;
//
// [CustomEditor(typeof(GameObject))]
// public class CheckColliderEditor : Editor
// {
//     [MenuItem("Tools/Check Collider in Folder")]
//     public static void CheckColliderIndex()
//     {
//         // Lấy folder được chọn
//         string[] guids = Selection.assetGUIDs;
//         if (guids.Length == 0)
//         {
//             Debug.LogWarning("Please select a folder in Project window!");
//             return;
//         }
//
//         string folderPath = AssetDatabase.GUIDToAssetPath(guids[0]);
//         if (!AssetDatabase.IsValidFolder(folderPath))
//         {
//             Debug.LogWarning("Selected item is not a folder!");
//             return;
//         }
//
//         // Tìm tất cả Prefab trong folder
//         string[] assetPaths = AssetDatabase.FindAssets("t:Prefab", new[] { folderPath });
//         foreach (string assetPath in assetPaths)
//         {
//             GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(assetPath));
//             if (prefab != null)
//             {
//                 if (prefab.GetComponent<Collider>() != null)
//                 {
//                     Debug.Log($"{prefab.name}");
//                     continue;
//                 }
//                 
//
//                 CheckGameObject(prefab, 1);
//             }
//         }
//
//         Debug.Log("Collider index 2 check completed!");
//     }
//
//     private static void CheckGameObject(GameObject go, int index)
//     {
//         if(go.transform.childCount == 0)
//         {
//             return; // Không có con nào, không cần kiểm tra
//         }
//         // Kiểm tra đệ quy các con của GameObject
//         for (int i = 0; i < go.transform.childCount; i++)
//         {
//             Transform child = go.transform.GetChild(i);
//             if (child.GetComponent<Collider>() != null && index != 1)
//             {
//                 Debug.Log($"{child.name} has collider at index {index} in {go.name}");
//                 return;
//             }
//             CheckGameObject(child.gameObject, index + 1);
//         }
//     }
// }