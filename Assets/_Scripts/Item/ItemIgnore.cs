using UnityEngine;


public class ItemIgnore : MonoBehaviour
{
    
    
    public int layerID = 7; // Default layer ID for items
    public void Awake()
    {
        SetIgnoreLayer();
    }

    private void SetIgnoreLayer()
    {
        Physics.IgnoreLayerCollision(layerID, layerID, true);
    }
}
