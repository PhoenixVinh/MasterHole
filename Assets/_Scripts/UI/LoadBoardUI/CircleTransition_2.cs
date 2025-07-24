
using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CircleTransition_2 : MonoBehaviour
{

    [SerializeField] private GameObject pannel;
    [SerializeField] private Image _backImage;


    private Material _matCircle;

    [Header("Timer Transition")]
    private float timeTransition = 1f;



    private static readonly int RADIUS = Shader.PropertyToID("_Radius");
    private static readonly int ASPECT_RATIO = Shader.PropertyToID("_AspectRatio");


    private float currentRadious = 0f;

    private void Awake()
    {
        DrawBlackScreen();
        _matCircle = _backImage.material;
        _matCircle.SetFloat(ASPECT_RATIO, (float)Screen.width / (float)Screen.height);

    }



    private void DrawBlackScreen()
    {
        _backImage.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
    }




    private IEnumerator TransitionCoroutine(float startRadius,float targetRadius)
    {
         _matCircle.SetFloat(RADIUS, startRadius);
        float duration = 0.6f;
        float elapsedTime = 0f;
      

        while (elapsedTime < duration)
        {
            elapsedTime += 0.01f; // Sử dụng unscaled để khớp với SetUpdate(true)
            float t = elapsedTime / duration; // Tỷ lệ thời gian (0 đến 1)
            float easedT = InQuad(t); // Tính toán ease InQuad

            currentRadious = Mathf.Lerp(startRadius, targetRadius, easedT);
            _matCircle.SetFloat(RADIUS, currentRadious);

            yield return new WaitForSecondsRealtime(0.01f); // Chờ frame tiếp theo
        }

        // Đảm bảo giá trị cuối cùng chính xác
        currentRadious = targetRadius;
        _matCircle.SetFloat(RADIUS, currentRadious);
        this.gameObject.SetActive(false);
    }
    private float InQuad(float t)
    {
        return t * t;
    }

    public void SetTransition()
    {
         pannel.SetActive(true);
        this.gameObject.SetActive(true);
        StartCoroutine(TransitionCoroutine(1.01f, 0));
    }

    public void ReverseTransition()
    {
        pannel.SetActive(false);
        this.gameObject.SetActive(true);
        StartCoroutine(TransitionCoroutine(0,1.01f));
        
    }
}