using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class ZoomOutBtn : MonoBehaviour
{
 



    private void OnEnable()
    {
        //StartCoroutine(ZoomOutButtonCoroutine());

        ShowAnim();
    }

    private void ShowAnim()
    {
        // for (int i = 0; i < 2; i++)
        // {
        //     ZoomOutButtonCoroutine(Vector3.one, Vector3.one * 0.8f, 0.1f);
        //     ZoomOutButtonCoroutine(Vector3.one * 0.8f, Vector3.one, 0.1f);
        // }

        transform.localScale = Vector3.one * 0.9f;
        var sequence = DOTween.Sequence()
            .SetId("AnimationZoomOutButton")
            .SetUpdate(true);


        sequence.Append(transform.DOScale(Vector3.one * 1.1f, 0.1f))
                .Append(transform.DOScale(Vector3.one * 0.95f, 0.2f))
                .Append(transform.DOScale(Vector3.one, 0.1f));
              
                


        
       
    }

    // private void ZoomOutButtonCoroutine()
    // {
    //     var targetLocalScale = transform.localScale;
    //     transform.localScale = Vector3.zero;
    //     DOTween.Sequence()
    //         .SetId("AnimationZoomOutButton")
    //         .SetUpdate(true)
    //         .Append(transform.DOScale(targetLocalScale * 1.1f, time))
    //         .Append(transform.DOScale(targetLocalScale, 0.1f));
    //         
    //
    //   
    //     transform.localScale = targetLocalScale;
    // }
    private IEnumerator ZoomOutButtonCoroutine(Vector3 start, Vector3 end, float time)
    {
        // First scale to targetLocalScale * 1.1f over 'time' seconds
        float elapsed = 0f;
        Vector3 startScale =start;
        Vector3 endScale = end;

        while (elapsed < time)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / time);
            transform.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null; // Wait for the next frame
        }
        transform.localScale = endScale; // Ensure final scale is exact

        // Then scale back to targetLocalScale over 0.1 seconds
        elapsed = 0f;
        startScale = transform.localScale;
        endScale =  Vector3.one;
        float returnTime = 0.1f;

        while (elapsed < returnTime)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / returnTime);
            transform.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null; // Wait for the next frame
        }
        transform.localScale = endScale; // Ensure final scale is exact
    }

    private void OnDisable()
    {
        DOTween.Kill("AnimationZoomOutButton");
        DOTween.KillAll();
        //DOTween.KillAll();
    }

   
}