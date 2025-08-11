using _Scripts.UI;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ButtonInputHandler : MonoBehaviour
{
    
    public TMP_InputField inputField; // Kéo InputField vào đây trong Inspector

    // Hàm được gọi khi nhấn Button
    public void ActivateInput()
    {
        if (inputField != null)
        {
            inputField.ActivateInputField(); // Kích hoạt InputField
            inputField.Select(); // Đảm bảo con trỏ nhấp nháy
        }
    }
}