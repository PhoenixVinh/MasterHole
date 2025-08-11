
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace _Scripts.HACK
{
    public class ChangeNearClipCameraNormal : MonoBehaviour
    {
        public TMP_InputField inputField;
        
        
        public UnityEngine.Camera vcam;
        
        public static ChangeNearClipCameraNormal instance;
    

        public void Start()
        {
            if (instance == null)
            {
               
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
            
            
        }

        public void Set()
        {
            vcam.nearClipPlane = float.Parse(inputField.text);
        }
    }
}