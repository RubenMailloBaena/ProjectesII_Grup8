using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    private static ColorManager instance;

    public static ColorManager Instace {
        get {
            if (instance is null)
                instance = FindObjectOfType<ColorManager>();
            return instance;
        }
    }

    [Header("GENERAL")] 
    [SerializeField] private GameObject roulettePrefab;
    [SerializeField] private Color waterColor;
    [SerializeField] private Color elasticColor;
    [SerializeField] private Color strechColor;
    [SerializeField] private Color defaultColor;
     private bool elasticAssigned;
    private bool waterAssigned;
    private bool strechAssigned;

    private IColorEffect DefaultObject;

    //Elastic
    [Header("ELASTIC")]
    [SerializeField] private float elasticMinImpulse;
    [SerializeField] private float elasticHeightMultiplier;
    [SerializeField] private float elasticDistanceMultiplier;

    //Stretch
    [Header("STRECH")]
    [SerializeField] private float stretchMultiplier;
    [SerializeField] private float inverseStrechMultiplier;
    [SerializeField] private float extendableSpriteVelocity;
    [SerializeField] private float collisionSize;
    [SerializeField] private float colYSize;
    [SerializeField] private LayerMask strechLayerMask;

    //Water
    [Header("WATER")]
    [SerializeField] private LayerMask waterLayerMask;

    private void Start()
    {
        DefaultObject = new DefaultEffect(Color.white, ColorType.Default);
        Instantiate(roulettePrefab, transform.position, Quaternion.identity);
    }

    public IColorEffect GetColorEffect(ColorType colorType)
    {
        switch (colorType){

            case ColorType.Elastic:
                if (elasticAssigned)
                    return DefaultObject;
                elasticAssigned = true;
                return new ElasticEffect(elasticColor, ColorType.Elastic, elasticMinImpulse, elasticHeightMultiplier,elasticDistanceMultiplier);

            case ColorType.Water:
                if (waterAssigned)
                    return DefaultObject;
                waterAssigned = true;
                return new WaterEffect(waterColor, ColorType.Water, waterLayerMask);

            case ColorType.Strech:
                if (strechAssigned)
                    return DefaultObject;
                strechAssigned = true;
                return new StrechEffect(strechColor, ColorType.Strech, stretchMultiplier, inverseStrechMultiplier, extendableSpriteVelocity, collisionSize, colYSize, strechLayerMask);

            case ColorType.Default:
                return DefaultObject;

            default:
                throw new ArgumentException("Color no soportado", nameof(colorType));
        }
    }

    public bool GetAssigneds(ColorType colorType)
    {
        switch (colorType)
        {
            case ColorType.Elastic:
                return elasticAssigned;
            case ColorType.Water:
                return waterAssigned;
            case ColorType.Strech:
                return strechAssigned;
            default:
                throw new ArgumentException("Color no soportado", nameof(colorType));
        }
    }

    public Color GetColor(ColorType colorType)
    {
        switch (colorType)
        {
            case ColorType.Elastic:
                if (elasticAssigned)
                    break;
                return elasticColor;
            case ColorType.Water:
                if (waterAssigned)
                    break;
                return waterColor;
            case ColorType.Strech:
                if (strechAssigned)
                    break;
                return strechColor;
        }
        return defaultColor;
    }

    private void ChangeAssigneds(ColorType colorType)
    {
        switch (colorType) { 
            case ColorType.Elastic:
                elasticAssigned = false;
                break;
            case ColorType.Water:
                waterAssigned = false;
                break;
            case ColorType.Strech:
                strechAssigned = false;
                break;
        }
    }

    private void OnEnable()
    {
        ObstacleEffectLogic.onChangeEffect += ChangeAssigneds;
    }

    private void OnDisable()
    {
        ObstacleEffectLogic.onChangeEffect -= ChangeAssigneds;
    }
}
