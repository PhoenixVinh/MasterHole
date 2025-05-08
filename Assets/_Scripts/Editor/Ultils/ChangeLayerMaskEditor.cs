using UnityEditor;
using UnityEngine;

namespace _Scripts.Editor.Ultils
{
    public class ChangeLayerMaskEditor : EditorWindow
    { 
        private string resourcesSubfolder = "PrefabInstance";
        private string tag = "Tag";
        private Vector2 scrollPosition;
        private int selectedLayer = 0;

    [MenuItem("Tools/ChangeLayerMaskEditor")]
    public static void ShowWindow()
    {
        GetWindow<ChangeLayerMaskEditor>("Change Layer Mask from Prefab");
    }

    private void OnGUI()
    {
        GUILayout.Label("Change Layer Mask from a Single Prefab", EditorStyles.boldLabel);
        GUILayout.Space(10);

        GUILayout.Label("Select Prefab:");
        resourcesSubfolder = EditorGUILayout.TextField("Resources Subfolder", resourcesSubfolder);
        
        GUILayout.Label("Layer Mask:");
        selectedLayer = EditorGUILayout.LayerField("Target Layer", selectedLayer);
        
        GUILayout.Label("Tag:");
        tag = EditorGUILayout.TagField("Tag", tag);
        
        
        
        GUILayout.Space(10);

        if (GUILayout.Button("Remove Missing Scripts"))
        {
            RemoveMissingScripts();
        }

        GUILayout.Space(10);
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
       
        GUILayout.EndScrollView();
    }

    private void RemoveMissingScripts()
    {
       
        try
        {
            
            GameObject[] prefabs = Resources.LoadAll<GameObject>(resourcesSubfolder);


            foreach (var prefab in prefabs)
            {
                int missingScriptsRemoved = ChangeLayerMaskGameObject(prefab);

                // Save the modified prefab
                EditorUtility.SetDirty(prefab);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                
                
            }
            

           
        }
        catch (System.Exception ex)
        {
            
        }
    }

    private int ChangeLayerMaskGameObject(GameObject prefab)
    {
        prefab.layer = selectedLayer;
        prefab.tag = tag;
        for (int i = 0; i < prefab.transform.childCount; i++)
        {
            prefab.transform.GetChild(i).gameObject.layer = selectedLayer;
            prefab.transform.GetChild(i).tag = tag;
        }
        return 1;
    }
        
        
        
        
        
        
        
    }
}