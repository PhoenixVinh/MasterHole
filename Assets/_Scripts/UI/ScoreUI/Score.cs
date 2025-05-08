
using System;
using System.Collections;
using _Scripts.ObjectPooling;
using TMPro;
using UnityEngine;


public class Score : MonoBehaviour
{
    [Header(" Variables")]
    public float _speedMovement;
    public float _timeToDistroy;
    public float _score;
    
    private TMP_Text text;


    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }


    public void OnEnable()
    {
        StartCoroutine(MovieTextCoroutine());
    }
    
    private IEnumerator MovieTextCoroutine()
    {

        text.text = $"+{_score}";
        //Set scale for text
        this.transform.localScale = Vector3.one * Mathf.Sqrt(_score);
        
        
        
        while (_timeToDistroy > 0)
        {
            _timeToDistroy -= Time.deltaTime;
            transform.Translate(Vector3.up*Time.deltaTime*_speedMovement);
            yield return null;
        }
        TextPooling.Instance .ReturnToPool(this.gameObject);
        
    }
    
    
    
    


    public void SetData(float speedMovement, float timeToDistroy, int score)
    {
        _speedMovement = speedMovement;
        _timeToDistroy = timeToDistroy;
        _score = score;
    }
    
    
    
    
    
    


}