using System;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    [Header("Parabola Points")]
    public Transform startPoint;
    public Transform highestPoint;
    public Transform endPoint;

    [Header("Movement Settings")]
    public float duration = 2f;
    private float timer = 0f;
    private bool isMoving = false;

    private bool isReverseMove = false;
    
    
    private RectTransform rectTransform;


    private float startIndexUI;
    private float endIndexUI;


    private void OnEnable()
    {
        rectTransform = GetComponent<RectTransform>();
        startIndexUI = startPoint.GetComponent<RectTransform>().anchoredPosition.x;
        endIndexUI = endPoint.GetComponent<RectTransform>().anchoredPosition.x;
        StartParabola();
    }

    [ContextMenu("Start Parabola")]
    public void StartParabola()
    {
        timer = 0f;
        isMoving = true;
    }


    void Update()
    {
        if (!isMoving) return;
        if(isReverseMove)
        {
            timer -= Time.unscaledDeltaTime;
           
        }
        else
        {
            timer += Time.unscaledDeltaTime;
        }
       
        float t = Mathf.Clamp01(timer / duration);

        Vector3 currentPos = GetParabolaPosition(t);
        transform.position = currentPos;

        // For UI: rotate the RectTransform to face the direction of movement along the curve
        if (t < 1f)
        {
            float lookAheadT = Mathf.Clamp01(t + 0.01f);
            Vector3 nextPos = GetParabolaPosition(lookAheadT);
            Vector3 direction = nextPos - currentPos;
            direction.z = 0; // UI is usually in XY plane
            if (direction.sqrMagnitude > 0.0001f)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
               
                if (rectTransform != null)
                {
                    rectTransform.rotation = Quaternion.Euler(0, 0, angle);
                }
            }
        }

        if (t >= 1f || t <= 0f)
        {
           
            isReverseMove = !isReverseMove; // Toggle direction for next move
        }

      

    }

    private Vector3 GetParabolaPosition(float t)
    {
        // Quadratic Bezier curve: B(t) = (1-t)^2 * P0 + 2(1-t)t * P1 + t^2 * P2
        return Mathf.Pow(1 - t, 2) * startPoint.position +
               2 * (1 - t) * t * highestPoint.position +
               Mathf.Pow(t, 2) * endPoint.position;
    }

    public int GetIndexPosition()
    {

        float distance = this.endIndexUI - this.startIndexUI;

        float range = distance / 5;
        
        return  (int)((this.rectTransform.anchoredPosition.x - this.startIndexUI) / range);
        
        
       
    }


    public void StopSpinner()
    {
        isMoving = false;
    }
}