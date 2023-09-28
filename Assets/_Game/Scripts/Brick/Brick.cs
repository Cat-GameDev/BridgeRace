using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField] ColorData colorData;
    [SerializeField] Renderer meshRenderer;
    public ColorType color;

    private void Start() 
    {
        color = GetRandomColor();
        ChangeColor(color);   
    }

    private ColorType GetRandomColor()
    {   
        int randomIndex = Random.Range(1, System.Enum.GetValues(typeof(ColorType)).Length);
        return (ColorType)randomIndex;
    }

    public void ChangeColor(ColorType colorType)
    {
        color = colorType;
        meshRenderer.material = colorData.GetMat(colorType);
    }


}
