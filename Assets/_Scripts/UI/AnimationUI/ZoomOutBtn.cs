using System;
using DG.Tweening;
using UnityEngine;

public class ZoomOutBtn : MonoBehaviour
{
    public float time = 0.5f;
    
    

    private void Start()
    {
        AnimationGameObjectZoomOut();
    }

    private void AnimationGameObjectZoomOut()
    {
        var targetLocalScale = transform.localScale;
        transform.localScale = Vector3.zero;
        DOTween.Sequence()
            .SetId("AnimationZoomOutButton")
            .SetUpdate(true)
            .Append(transform.DOScale(targetLocalScale * 1.1f, time))
            .Append(transform.DOScale(targetLocalScale, 0.1f));




    }

    private void OnDisable()
    {
        DOTween.Kill("AnimationZoomOutButton");
        DOTween.KillAll();
    }

   
}