using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI.ProfileUI
{
    public class ProfileHomeScene : MonoBehaviour
    {
        
        public Image ProfileImage;
        
        
        public void Start()
        {
            UpdateProfile();
        }

        public void UpdateProfile()
        {
            int index = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_INDEX_PROFILE, 0);
            Sprite sprite = Resources.Load<Sprite>($"AvataProfile/av{index + 1}");
            ProfileImage.sprite = sprite;
        }
    }
}