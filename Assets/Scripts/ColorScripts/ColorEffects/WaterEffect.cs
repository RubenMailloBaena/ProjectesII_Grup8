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
    private SpriteRenderer sp;
    private Material waterMaterial;
    private Material prevMaterial;

    public static event Action onWater;

    public WaterEffect(Color color, ColorType colorType, LayerMask waterLayermask, Material waterMaterial) { 
        effectColor = color;
        this.colorType = colorType;
        this.waterLayermask = waterLayermask;
        this.waterMaterial = waterMaterial;
        this.prevMaterial = prevMaterial;
    }

    public void InitializeEffect(GameObject target)
    {
        GetAllObstaclesParts(target);
        previousColor = colorPartSprite.color;
        colorPartSprite.color = effectColor;
        actualTarget.GetComponent<BoxCollider2D>().isTrigger = true;
        sp = target.transform.parent.transform.Find("PlatformSprite").GetComponent<SpriteRenderer>();
        prevMaterial = sp.material;
        sp.drawMode = SpriteDrawMode.Sliced;
        sp.material = waterMaterial;
        Color x = sp.color;
        x.a = 0.1f;
        sp.color = x;
    }

    private void GetAllObstaclesParts(GameObject target)
    {
        actualTarget = target;
        colorPartSprite = actualTarget.transform.parent.transform.Find("PlatformSprite").GetComponent<SpriteRenderer>();
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
        sp.material = prevMaterial;
        sp.drawMode = SpriteDrawMode.Tiled; 
        Color x = sp.color;
        x.a = 1;
        sp.color = x;
    }

    public ColorType getColorType()
    {
        return colorType;
    }
}
