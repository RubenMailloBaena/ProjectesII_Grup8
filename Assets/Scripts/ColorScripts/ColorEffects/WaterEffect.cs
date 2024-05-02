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

        //COMPROBEN QUE AL INICIALITZAR EL JUGADOR NO ESTIGUI A SOBRE
        //Collider2D[] collider = Physics2D.OverlapBoxAll(target.transform.position, target.transform.localScale, 0f);

        //for (int i = 0; i < collider.Length; i++)
        //{
        //    if (collider[i].gameObject.CompareTag("Player"))
        //    {
        //        Debug.Log("INITIALIZE WATER");
        //        onWater?.Invoke();
        //    }
        //}
    }

    public void ApplyEffect()
    {
        Collider2D[] collider = Physics2D.OverlapBoxAll(actualTarget.transform.position, actualTarget.transform.localScale, 0f, waterLayermask);

        for (int i = 0; i < collider.Length; i++)
        {
            if (collider[i].gameObject.CompareTag("Player"))
            {
                Debug.Log("INITIALIZE WATER");
                onWater?.Invoke();
            }
        }
        //Debug.Log("Apply EFFECT WATER");
        //onWater?.Invoke();
    }

    public void RemoveEffect(GameObject target)
    {
        //Collider2D[] collider = Physics2D.OverlapBoxAll(target.transform.position, target.transform.localScale, 0f);

        //for (int i = 0; i < collider.Length; i++) {
        //    if (collider[i].gameObject.CompareTag("Player")) {
        //        onWater?.Invoke();
        //        Debug.Log("NO WATER");
        //    }
        //}
        target.GetComponent<SpriteRenderer>().color = previousColor;
        target.GetComponent<BoxCollider2D>().isTrigger = false;
    }

    public ColorType getColorType()
    {
        return colorType;
    }
}
