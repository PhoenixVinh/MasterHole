using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


public abstract class BaseShapeEditor : EditorWindow
{
    protected GameObject prefabToSpawn;
    
    public Transform parentObject;
    protected GameObject previewParent;
    protected List<GameObject> previewObjects = new List<GameObject>();

    
    protected void OnSelectionChange()
    {
        if (Selection.activeTransform != null)
        {
            parentObject = Selection.activeTransform;
            Repaint(); // Force the editor window to redraw and update the parent object field
        }
        else
        {
            parentObject = null;
            Repaint();
        }
    }
    
    protected void OnDisable()
    {
        // Clean up the preview objects and parent
        if (previewParent != null)
        {
            DestroyImmediate(previewParent);
            previewParent = null;
        }
        previewObjects.Clear();
    }
    protected void OnEnable()
    {
        // Create a persistent parent object for the preview
        previewParent = new GameObject("Preview Parent");
        previewParent.hideFlags = HideFlags.DontSaveInBuild | HideFlags.DontSaveInEditor;
    }




}