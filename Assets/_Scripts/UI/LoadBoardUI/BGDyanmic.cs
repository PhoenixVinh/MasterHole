using UnityEngine;
using UnityEngine.UI;

public class BGDyanmic : MonoBehaviour
{
    public RawImage rawImage;

    public float speed = 0;

    public void Update()
    {
        Rect uvRect = rawImage.uvRect;

        uvRect.x -= speed * Time.unscaledDeltaTime;
        
        rawImage.uvRect = uvRect;
    }
}