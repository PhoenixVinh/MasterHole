using _Scripts.HACK;
using _Scripts.UI;
using UnityEditor;
using UnityEngine;

namespace _Scripts.HACK
{
    public class CheckTestAll : MonoBehaviour
    {
        [SerializeField]private bool isTesting;


        public void SetTurnOn()
        {
            isTesting = true;
            PlayerPrefs.SetInt(StringPlayerPrefs.ISTESTGAME, 1);
        }

        public void SetTurnOff()
        {
            isTesting = false;
            PlayerPrefs.SetInt(StringPlayerPrefs.ISTESTGAME, 0);
            
        }
        
        
    }
    
    

}
#if UNITY_EDITOR
[CustomEditor(typeof(CheckTestAll))]
public class CheckTestAllEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CheckTestAll checkTestAll = (CheckTestAll)target;

        GUILayout.Space(10);
        if (GUILayout.Button("Set Turn On"))
        {
            checkTestAll.SetTurnOn();
        }
        if (GUILayout.Button("Set Turn Off"))
        {
            checkTestAll.SetTurnOff();
        }
    }
}
# endif