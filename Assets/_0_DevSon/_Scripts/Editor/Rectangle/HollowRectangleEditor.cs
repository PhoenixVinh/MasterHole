using UnityEditor;
using UnityEngine;

public class HollowRectangleEditor : RectangleEditor
{
    [MenuItem("Tools/Shape Editor/Rectangle/Hollow Rectangle")]
    public static void ShowWindow()
    {
        GetWindow<HollowRectangleEditor>("Rectangle Editor");
    }


    protected override void UpdatePreview()
    {
        if (prefabToSpawn == null) return;

        // Clear existing preview objects
        foreach (GameObject obj in previewObjects)
        {
            if (obj != null)
            {
                DestroyImmediate(obj);
            }
        }

        previewObjects.Clear();

        Vector3 centerPosition = parentObject.transform.position;
        // Calculate distance From (0,0) to the center of Rectangle 

        var z = row % 2 == 0 ? distance * (row / 2f - 0.5) : distance * Mathf.FloorToInt(row / 2f);
        var x = col % 2 == 0 ? distance * (col / 2f - 0.5) : distance * Mathf.FloorToInt(col / 2f);
        Vector3 distanceCenter = new Vector3((float)x, 0, (float)z);


        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                if (i == 0 || j == 0 || i == row - 1 || j == col - 1)
                {
                    Vector3 position = new Vector3(centerPosition.x + distance * j, centerPosition.y,
                        centerPosition.z + distance * i);

                    GameObject previewObject = PrefabUtility.InstantiatePrefab(prefabToSpawn) as GameObject;

                    if (previewObject != null)
                    {
                        previewObject.transform.position = position - distanceCenter;
                        previewObject.transform.SetParent(previewParent.transform);
                        previewObject.hideFlags = HideFlags.HideAndDontSave; // Hide in hierarchy and don't save
                        previewObjects.Add(previewObject);
                    }
                }
            }
        }


    }

    protected override void GenerateObjectsInRectangle()
    {
        if (prefabToSpawn == null) return;

        Vector3 centerPosition = parentObject.transform.position;
        
        var z = row % 2 == 0 ? distance * (row /2f - 0.5) : distance * Mathf.FloorToInt(row /2f) ;
        var x = col % 2 == 0 ? distance * (col /2f - 0.5) : distance * Mathf.FloorToInt(col /2f) ;
        Vector3 distanceCenter = new Vector3((float)x , 0, (float)z);
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {

                if (i == 0 || j == 0 || i == row - 1 || j == col - 1)
                {
                    Vector3 position = new Vector3(centerPosition.x + distance * j, centerPosition.y,
                        centerPosition.z + distance * i) - distanceCenter;  
                    GameObject newObject = Instantiate(prefabToSpawn, position, Quaternion.identity);
                    if (parentObject != null)
                    {
                        newObject.transform.SetParent(parentObject);
                    }
                }
              
            }
        }


    
       

        // Clean up the preview after generation
        OnDisable();
        OnEnable(); // Re-enable for future previews
    }
        
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
}