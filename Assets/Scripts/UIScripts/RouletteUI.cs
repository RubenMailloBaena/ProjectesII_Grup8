using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine.Utility;
using UnityEngine;
using UnityEngine.UI;

public class RouletteUI : MonoBehaviour
{
    private static RouletteUI instance;
    public static RouletteUI Instance {
        get {
            if (instance is null)
                instance = FindAnyObjectByType<RouletteUI>();
            return instance;
        }
    }

    private Image[] colors = new Image[3];
    private GameObject arrow;
    private int arrowAngle;
    [SerializeField] private float arrowTurnSpeed;

    private ColorType[] playerColors;
    private int colorsToPaint = 0;
    private Color defaultColor;

    private ColorManager colorManager;

    private void Awake()
    {
        colorManager = ColorManager.Instace;
        GetAllReferences();
    }

    private void Update()
    {
        ArrowLogic();
    }

    private void GetAllReferences()
    {
        for (int i = 1; i <= 3; i++)
            colors[i-1] = transform.Find("Color" + i).GetComponent<Image>();
        arrow = transform.Find("ArrowPivot").gameObject;
        defaultColor = colorManager.GetColor(ColorType.Default);
    }

    public void InitializeRouletteColors(ColorType[] colorTypes)
    {
        colorsToPaint = colorTypes.Length;
        playerColors = colorTypes;
        for (int i = 0; i < colors.Length; i++)
        {
            if (i < colorTypes.Length)
                colors[i].color = colorManager.GetColor(playerColors[i]);
            else
                colors[i].color = defaultColor;
        }
    }

    public void RepaintRoulette(int colorIndex)
    {
        for (int i = 0; i < colorsToPaint; i++)
            colors[i].color = colorManager.GetColor(playerColors[i]);

        arrowAngle = 0;
        switch (colorIndex)
        {
            case 1:
                arrowAngle = 120;
                break;
            
            case 2:
                arrowAngle = 240;
                break;
        }
        CheckIfAllColorsDefault();
    }

    private void ArrowLogic()
    {
        Quaternion currentRotation = arrow.transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0, 0, arrowAngle);

        Quaternion newRotation = Quaternion.Lerp(currentRotation, targetRotation, arrowTurnSpeed * Time.deltaTime);

        arrow.transform.rotation = newRotation;
    }

    private void CheckIfAllColorsDefault()
    {
        bool allGrey = true;
        for (int i = 0; i < colors.Length; i++)
        {
            if (colors[i].color != defaultColor)
            {
                allGrey = false;
                break;
            }
        }
        arrow.SetActive(true);
        if (allGrey)
            arrow.SetActive(false);
    }
}
