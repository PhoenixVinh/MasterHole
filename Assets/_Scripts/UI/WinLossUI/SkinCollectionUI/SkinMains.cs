using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

using _Scripts.UI;
using Assets._Scripts.UI.AnimationUI;

public class SkinMains : MonoBehaviour
{
    public GameObject RenderTexture;

    public List<GameObject> skinRenderTextures;
    public List<GameObject> skinMains;
    public GameObject SkinMain;
    public RawImage RenderTextureImage;

    public HoleAnimation HoleAnimation;



    public Button EquipButton;
    public Button ConitnueButton;


    private int currentSkinIndex = 0;


    public void OnEnable()
    {
        RenderTexture.gameObject.SetActive(true);
        EquipButton.onClick.AddListener(EquipSkin);
        ConitnueButton.onClick.AddListener(ContinueGame);
        HoleAnimation.MoveFruitsInCircleThenToTarget();
        StartCoroutine(ShowSkinMains());
    }
    public void OnDisable()
    {
        RenderTexture.gameObject.SetActive(false);
        EquipButton.onClick.RemoveAllListeners();
        ConitnueButton.onClick.RemoveAllListeners();
    }
    private void SetAllNoActive(List<GameObject> objects)
    {
        foreach (GameObject obj in objects)
        {
            obj.SetActive(false);
        }
    }

    private IEnumerator ShowSkinMains()
    {

        yield return new WaitForSecondsRealtime(5.5f);
        RenderTextureImage.gameObject.SetActive(false);
        
        SkinMain.SetActive(true);

    }

    public void EquipSkin()
    {
        
        PlayerPrefs.SetInt(StringPlayerPrefs.HOLESKININDEX, currentSkinIndex);
        PlayerPrefs.Save();
        this.EquipButton.gameObject.SetActive(false);
       
    }
    public void ContinueGame()
    {

        this.gameObject.SetActive(false);
        ManagerLevelGamePlay.Instance.LoadNextLevel();
    }

    public void SetData(int index)
    {
        currentSkinIndex = index;
        SetAllNoActive(skinRenderTextures);
        SetAllNoActive(skinMains);
        skinRenderTextures[index].SetActive(true);
        skinMains[index-1].SetActive(true);
        RenderTextureImage.gameObject.SetActive(true);
        SkinMain.SetActive(false);
        this.EquipButton.gameObject.SetActive(true);
        this.gameObject.SetActive(true);
    }
   
}