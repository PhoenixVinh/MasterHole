using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GetNumberObject))]
public class GetNumberObjectEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GetNumberObject targetObject = (GetNumberObject)target;
        if (GUILayout.Button("Get Number"))
        {
            targetObject.GetObjectsInfo();
        }

        if (GUILayout.Button("Assign Sprites"))
        {
            targetObject.AssignSprites();
        }

        if (GUILayout.Button("Sort Ascending"))
        {
            targetObject.SortAscending();
        }

        if (GUILayout.Button("Sort Descending"))
        {
            targetObject.SortDescending();
        }
    }
   

}
