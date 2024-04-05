using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterEffect : IColorEffect
{
    private Color effectColor;

    public WaterEffect(Color color) { 
        effectColor = color;
    }

    public void ApplyEffect(GameObject target)
    {
        //aplicar la logica del efecte
        target.GetComponent<SpriteRenderer>().color = effectColor;
    }
}
