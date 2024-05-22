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
    private SpriteRenderer colorPartSprite;

    public static event Action onWater;

    public WaterEffect(Color color, ColorType colorType, LayerMask waterLayermask) { 
        effectColor = color;
        this.colorType = colorType;
        this.waterLayermask = waterLayermask;
    }

    public void InitializeEffect(GameObject target)
    {
        GetAllObstaclesParts(target);
        previousColor = colorPartSprite.color;
        colorPartSprite.color = effectColor;
        actualTarget.GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private void GetAllObstaclesParts(GameObject target)
    {
        actualTarget = target;
        colorPartSprite = actualTarget.transform.parent.transform.Find("ColorChange").GetComponent<SpriteRenderer>();
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
        if (effectApplied)
        {   
            onWater?.Invoke();
            effectApplied = false;
        }

        colorPartSprite.color = previousColor;
        target.GetComponent<BoxCollider2D>().isTrigger = false;
    }

    public ColorType getColorType()
    {
        return colorType;
    }
}
