using System;
using _Scripts.UI.HomeSceneUI.ShopUI.TreasureUI;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

namespace _Scripts.UI.HomeSceneUI.ShopUI.TreasureUI
{
    public class ChestAnim: MonoBehaviour
    {
        public GameObject OffChest;
        public GameObject OnChest;
        
        
        public void OnEnable()
        {
           
            //PlayChestAnim();

        }
        
        
        public void PlayChestAnim()
        {
            
            OffChest.SetActive(true);
            OnChest.SetActive(false);
            OffChest.transform.DOShakeScale(1f,0.5f)
                .SetUpdate(true)
                .OnComplete(()=>
                {
                    OffChest.SetActive(false);
                    OnChest.SetActive(true);
                }
           
                );
          
            
        }

        public void OnDisable()
        {
            DOTween.Kill("ChestAnim");
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(ChestAnim))]
public class ChestAnimEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ChestAnim chestAnim = (ChestAnim)target;

        GUILayout.Space(10);
        if (GUILayout.Button("Test PlayChestAnim"))
        {
            chestAnim.PlayChestAnim();
        }
    }
}
#endif