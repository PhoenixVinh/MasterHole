using UnityEditor;
using UnityEngine;



public class HollowRectangleWithCenterEditor : HollowRectangleEditor
{
    public GameObject prefabCenterToSpawn;
    
    [MenuItem("Tools/Shape Editor/Rectangle/Hollow Rectangle With Center")]
    public static void ShowWindow()
    {
        GetWindow<HollowRectangleWithCenterEditor>("Rectangle Editor");
    }


    protected override void OnGUI()
    {
        prefabCenterToSpawn = (GameObject)EditorGUILayout.ObjectField("Prefab Center to Spawn", prefabCenterToSpawn, typeof(GameObject), false);
        base.OnGUI();
    }

    protected override void UpdatePreview()
    {
        base.UpdatePreview();
        GameObject centerObject = PrefabUtility.InstantiatePrefab(prefabCenterToSpawn) as GameObject;
        if (centerObject != null)
        {
            centerObject.transform.position = parentObject.transform.position;
            centerObject.transform.SetParent(previewParent.transform);
            centerObject.hideFlags = HideFlags.HideAndDontSave; // Hide in hierarchy and don't save
            previewObjects.Add(centerObject);
        }

    }
    
    
    protected override void GenerateObjectsInRectangle()
    {
    
        if (prefabCenterToSpawn == null) return;
        GameObject newObject = Instantiate(prefabCenterToSpawn, parentObject.transform.position, Quaternion.identity);
        newObject.transform.SetParent(parentObject);
        base.GenerateObjectsInRectangle();
    }
    
    
    
}


