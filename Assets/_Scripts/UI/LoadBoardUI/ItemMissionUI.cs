
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ItemMissionUI : MonoBehaviour
{
    public Image MainImage;
    public Image MainImageBG;

    public TMP_Text txtAmount;


    public void SetData(Sprite sprite, int amount)
    {
        MainImage.sprite = sprite;
        MainImageBG.sprite = sprite;
        txtAmount.text = amount.ToString();
    }



}
