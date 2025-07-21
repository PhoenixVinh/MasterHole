using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Ads;
using UnityEngine;



public class BoardGameUI : MonoBehaviour
{
    public Transform MissionContainer;
    public GameObject MissionContainerPrefab;

    public void SetData(MissionSO data)
    {
        RemoveOldData();
        foreach (var item in data.misstionsData)
        {
            GameObject itemMission = Instantiate(MissionContainerPrefab, MissionContainer);
            itemMission.GetComponent<ItemMissionUI>().SetData(item.image, item.AmountItems);

        }

        this.gameObject.SetActive(true);


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
        
    }

    private void OnDisable() {
        StopAllCoroutines();
    }
    public void HideUI()
    {
        StartCoroutine(TurnOffBoardGameUI());
    }


    private IEnumerator TurnOffBoardGameUI()
    {
        yield return new WaitForSeconds(1.5f);
        this.gameObject.SetActive(false);

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
    }


}
