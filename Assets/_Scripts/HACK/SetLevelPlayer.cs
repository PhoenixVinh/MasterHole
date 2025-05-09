using _Scripts.UI;
using UnityEngine;

namespace _Scripts.HACK
{
    public class SetLevelPlayer : MonoBehaviour
    {
        public int level = 1;

        [ContextMenu("SetLevel")]
        public void SetLevelHole()
        {
            PlayerPrefs.SetInt(StringPlayerPrefs.CURRENT_LEVEL, level);
        }
    }
}