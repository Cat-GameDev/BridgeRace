using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickInBridge : Brick
{
    [SerializeField] private BoxCollider boxCollider;
    private void Start() 
    {
        color = ColorType.None;
        ChangeColor(color);   
        boxCollider.enabled = false;
    }

    public void EnableBoxCollider()
    {
        boxCollider.enabled = true; 
    }

    public void DisableBoxCollider()
    {
        boxCollider.enabled = false; 
    }

  
    



}
