using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.HACK
{
    public class ChangeNearClipPlane : MonoBehaviour
    {
        public TMP_InputField inputField;
        
        
        public CinemachineVirtualCamera vcam;

        public void Set()
        {
            vcam.m_Lens.NearClipPlane = float.Parse(inputField.text);
        }
    }
}