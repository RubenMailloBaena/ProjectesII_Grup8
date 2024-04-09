using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    [SerializeField] private Color waterColor;
    [SerializeField] private Color elasticColor;
    [SerializeField] private Color strechColor;

    [SerializeField] private float elasticMinImpulse;
    [SerializeField] private float elasticHeightMultiplier;

    public IColorEffect GetColorEffect(ColorType colorType)
    {
        switch (colorType){

            case ColorType.Elastic:
                return new ElasticEffect(elasticColor, ColorType.Elastic, elasticMinImpulse, elasticHeightMultiplier);

            case ColorType.Water:
                return new WaterEffect(waterColor, ColorType.Water);

            case ColorType.Strech:
                return new StrechEffect(strechColor, ColorType.Strech);

            default:
                throw new ArgumentException("Color no soportado", nameof(colorType));
        }
    }

    public Color GetColor(ColorType colorType) {
        switch (colorType)
        {

            case ColorType.Elastic:
                return elasticColor;

            case ColorType.Water:
                return waterColor;

            case ColorType.Strech:
                return strechColor;

            default:
                throw new ArgumentException("Color no soportado", nameof(colorType));
        }
    }
}
