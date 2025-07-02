using UnityEditor;
using UnityEngine;

public class CheckFile
{
    [MenuItem("Tools/Check Missing Mesh")]
    public static void CheckMissingMeshEditor()
    {
        string folderPath = "Assets/Resources/PrefabInstance/GameObject/"; // Đường dẫn đến thư mục chứa prefab
        
        
        string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { folderPath });

        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);

            bool hasMissingMesh = false;
            foreach (var meshFilter in prefab.GetComponentsInChildren<MeshFilter>(true))
            {
                if (meshFilter.sharedMesh == null)
                {
                    hasMissingMesh = true;
                    break;
                }
            }
            if (!hasMissingMesh)
            {
                foreach (var meshRenderer in prefab.GetComponentsInChildren<MeshRenderer>(true))
                {
                    if (meshRenderer.sharedMaterial == null)
                    {
                        hasMissingMesh = true;
                        break;
                    }
                }
            }
            if (hasMissingMesh)
            {
                Debug.Log($"Missing mesh in prefab: {prefab.name}");
            }
        }
    }

}
