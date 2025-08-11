using UnityEngine;
using UnityEngine.UI;

public class UIScaler : MonoBehaviour
{

    public Canvas mainCanvas;
    private void Awake()
    {
        Debug.Log(Screen.width + "|||" + Screen.height);
        float match = Screen.height / (float)Screen.width >= 16.0/9  ? 0 : 1;
        mainCanvas.GetComponent<CanvasScaler>().matchWidthOrHeight = match;
        //mainCanvas.ma
    }
}