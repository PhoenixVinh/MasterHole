using UnityEditor;
using UnityEngine;

public class RemoveMissingScriptsFromPrefabEditor : EditorWindow
{
    private string resourcesSubfolder = "PrefabInstance";
    private string statusMessage = "";
    private Vector2 scrollPosition;

    [MenuItem("Tools/Remove Missing Scripts from Prefab")]
    public static void ShowWindow()
    {
        GetWindow<RemoveMissingScriptsFromPrefabEditor>("Remove Missing Scripts from Prefab");
    }

    private void OnGUI()
    {
        GUILayout.Label("Remove Missing Scripts from a Single Prefab", EditorStyles.boldLabel);
        GUILayout.Space(10);

        GUILayout.Label("Select Prefab:");
        resourcesSubfolder = EditorGUILayout.TextField("Resources Subfolder", resourcesSubfolder);

        GUILayout.Space(10);

        if (GUILayout.Button("Remove Missing Scripts"))
        {
            RemoveMissingScripts();
        }

        GUILayout.Space(10);
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        GUILayout.Label(statusMessage, EditorStyles.wordWrappedLabel);
        GUILayout.EndScrollView();
    }

    private void RemoveMissingScripts()
    {
        statusMessage = "";
        try
        {
            
            //GameObject[] prefabs = Resources.LoadAll<GameObject>(resourcesSubfolder);
            string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { resourcesSubfolder });
            GameObject[] prefabs = new GameObject[guids.Length];

            for (int i = 0; i < guids.Length; i++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                prefabs[i] = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
            }

            Debug.Log(prefabs.Length);
            foreach (var prefab in prefabs)
            {
                int missingScriptsRemoved = RemoveMissingScriptsFromPrefab(prefab);

                // Save the modified prefab
                EditorUtility.SetDirty(prefab);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                
                if (missingScriptsRemoved > 0)
                {
                    statusMessage = $"Success: Removed {missingScriptsRemoved} missing scripts from {prefab.name} .";
                }
                else
                {
                    statusMessage = $"No missing scripts found on {prefab.name} at .";
                }
            }
            

           
        }
        catch (System.Exception ex)
        {
            statusMessage = "Error: " + ex.Message;
        }
    }

    private int RemoveMissingScriptsFromPrefab(GameObject prefab)
    {
        var components = prefab.GetComponents<Component>();
        for (int i = 0; i < components.Length; i++)
        {
            if (components[i] == null)
            {
                GameObjectUtility.RemoveMonoBehavioursWithMissingScript(prefab);
            }
        }
        
        if(prefab.transform.childCount > 0)
        {
            foreach (Transform child in prefab.transform)
            {
                RemoveMissingScriptsFromPrefab(child.gameObject);
            }
        }
        
        

        return 1;
    }
}