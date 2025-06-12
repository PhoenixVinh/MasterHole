using TMPro;
using UnityEngine;

namespace _Scripts.UI
{
    public class DisplayVersion_UI : MonoBehaviour
    {
        public TMP_Text versionText;
        public void OnEnable()
        {
            versionText.text = "Version " + Application.version;
        }
    }
}