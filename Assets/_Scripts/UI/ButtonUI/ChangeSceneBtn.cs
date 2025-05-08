using System;
using System.Collections;
using _Scripts.ManagerScene;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeSceneBtn : MonoBehaviour
{
    public EnumScene namescene;
    
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ChangeScene);
    }

    public virtual void ChangeScene()
    {

        StartCoroutine(ChangeSceneAsyns());

    }

    IEnumerator ChangeSceneAsyns()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(namescene.ToString());
        
        
        SceneManager.LoadScene(namescene.ToString());

        while (!asyncOperation.isDone)
        {
            yield return null;
        }
    }
}