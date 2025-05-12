using System;
using _Scripts.Event;
using Unity.VisualScripting;
using UnityEngine;

public class HoleLevel : MonoBehaviour, IPrecent
{
    private int expToLevelUp;
    private int currentExp;
    
    // Update UI for Level up 

    private void OnEnable()
    {
        ItemEvent.OnAddScore += AddExp;    
    }

    private void OnDisable()
    {
        ItemEvent.OnAddScore -= AddExp;
    }

    private void AddExp(int score)
    {
        currentExp += score;
        if (currentExp >= expToLevelUp)
        {
            HoleEvent.OnLevelUp?.Invoke();
        }
    }

    public void SetData(int amountExp)
    {
        this.expToLevelUp = amountExp;
        this.currentExp = 0;
    }
    
    
    // Add UI 
    
    

    [ContextMenu("Level up")]
    public void UpLoadLevel()
    {
        //Change radious of Hole 
        Vector3 localScale = transform.localScale;
        Vector3 newScale = new Vector3(localScale.x * 1.2f, localScale.y, localScale.z * 1.2f);
        
        
        
        // Update Scale of Hole 
        this.transform.localScale = newScale;
        //HoleController.Instance.OnUpLevelHole();
        
        
    }

    public float Precent()
    {
        return currentExp / (float)expToLevelUp;
    }
}