using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterEffect : IWaterEffect
{
    private Color effectColor;
    private ColorType colorType;
    private LayerMask waterLayermask;
    private Color previousColor;
    private GameObject actualTarget;
    private bool effectApplied = false;
    private Collider2D[] collisions;

    public static event Action onWater;

    public WaterEffect(Color color, ColorType colorType, LayerMask waterLayermask) { 
        effectColor = color;
        this.colorType = colorType;
        this.waterLayermask = waterLayermask;
    }

    public void InitializeEffect(GameObject target)
    {
        actualTarget = target;
        previousColor = actualTarget.GetComponent<SpriteRenderer>().color;
        actualTarget.GetComponent<SpriteRenderer>().color = effectColor;
        actualTarget.GetComponent<BoxCollider2D>().isTrigger = true;
    }

    public void ApplyEffect(bool enterOnTrigger)
    {
        if(enterOnTrigger && !effectApplied){
            onWater?.Invoke();
            effectApplied = true;
        }
        else if (!enterOnTrigger && effectApplied) {
            onWater?.Invoke();
            effectApplied = false;
        }
    }

    public void RemoveEffect(GameObject target)
    {
        collisions = Physics2D.OverlapBoxAll(actualTarget.transform.position, actualTarget.transform.localScale, 0f, waterLayermask);
        if (collisions.Length != 0 && effectApplied)
        {   
            onWater?.Invoke();
            effectApplied = false;
        }

        target.GetComponent<SpriteRenderer>().color = previousColor;
        target.GetComponent<BoxCollider2D>().isTrigger = false;
    }

    public ColorType getColorType()
    {
        return colorType;
    }
}
