using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CreateColiderHand))]
public class CreateColliderHandEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CreateColiderHand myScript = (CreateColiderHand)target;
        if (GUILayout.Button("Create Collider"))
        {
            myScript.CreateCapsule();
        }
    }
}
