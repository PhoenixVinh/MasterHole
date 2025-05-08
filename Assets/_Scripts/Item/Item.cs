using System;
using Unity.VisualScripting;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int score  = 1;
    public string type = "food";
    

    public void SetData(string foodName, int score)
    {
        this.score = score;
        this.type = foodName;
    }
}