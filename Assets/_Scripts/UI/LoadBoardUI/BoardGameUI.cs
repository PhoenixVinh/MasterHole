using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Ads;
using DG.Tweening;
using TMPro;
using UnityEngine;



public class BoardGameUI : MonoBehaviour
{
    public Transform MissionContainer;
    public GameObject MissionContainerPrefab;

    public TMP_Text timmerComplete; 
    public bool isShowBoardGame = false;


    [Header("Mission Frame")]
    [SerializeField] private RectTransform frameUI;

    public void SetData(MissionSO data, float timeComplete)
    {
        RemoveOldData();
        timmerComplete.text = timeComplete.ToString();
        foreach (var item in data.misstionsData)
        {
            GameObject itemMission = Instantiate(MissionContainerPrefab, MissionContainer);
            itemMission.GetComponent<ItemMissionUI>().SetData(item.image, item.AmountItems);

        }
        this.gameObject.SetActive(true);
        ShowAnimEnable();

        //StartCoroutine(ShowFadeLoading());
       


    }

    private IEnumerator ShowFadeLoading()
    {
        yield return null;
    }

    public void RemoveOldData()
    {
        while (MissionContainer.childCount > 0)
        {
            DestroyImmediate(MissionContainer.GetChild(0).gameObject);
        }
    }

    public void OnEnable()
    {
        StartCoroutine(CheckCMP());
        isShowBoardGame = true;
       
        
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        isShowBoardGame = false;
       
    }
    public void HideUI()
    {
        StartCoroutine(TurnOffBoardGameUI());
    }

    [ContextMenu("Show Anim")]
    public void ShowAnimEnable()
    {
        Vector2 originAnchor = Vector2.one;
        float maxHeight = Screen.height;
        Vector2 startAnchor = originAnchor + new Vector2(0,maxHeight+ 500);
        frameUI.anchoredPosition = startAnchor;
        frameUI.DOAnchorPosY(originAnchor.y, 0.6f);

    }


    private IEnumerator TurnOffBoardGameUI()
    {
        yield return new WaitForSeconds(3f);
        //ShowAnimDisable();
        yield return new WaitForSeconds(1f);
        Debug.Log("???");
        //this.gameObject.SetActive(false);

    }

     [ContextMenu("Show Anim")]
    public void ShowAnimDisable()
    {
        float targetAnchorY = -Screen.height - 500;
        frameUI.DOAnchorPosY(targetAnchorY, 1f);

    }

    private IEnumerator CheckCMP()
    {


        yield return new WaitForSeconds(0.3f);

        Time.timeScale = 0;
        if (CMPController.Instance != null)
        {
            CMPController.Instance.StarCMP();
            while (!CMPController.Instance.IsShowCMP)
            {
                yield return null;
            }
        }


        Time.timeScale = 1;
        ShowAnimEnable();
    }


}
