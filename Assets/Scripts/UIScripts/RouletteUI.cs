using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine.Utility;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RouletteUI : MonoBehaviour
{
    private Image[] colors = new Image[3];
    private GameObject arrow;
    private int arrowAngle;
    [SerializeField] private float arrowTurnSpeed;
    private bool foundAllReferences;

    private ColorType[] playerColors;
    private int colorsToPaint = 0;
    private Color defaultColor;

    private ColorManager colorManager;

    private void Update()
    {
        if (foundAllReferences) 
            ArrowLogic();
    }

    public void InitializeRouletteColors(ColorType[] colorTypes)
    {
        colorManager = FindAnyObjectByType<ColorManager>();
        GetAllReferences();
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
    
    private void GetAllReferences()
    {
        for (int i = 1; i <= 3; i++)
            colors[i-1] = transform.Find("Color" + i).GetComponent<Image>();
        arrow = transform.Find("ArrowPivot").gameObject;
        defaultColor = colorManager.GetColor(ColorType.Default);
        foundAllReferences = true;
    }

    public void RepaintRoulette(int colorIndex)
    {
        if(!foundAllReferences)
            InitializeRouletteColors(TongueController.Instance.GetColorTypes());
        
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
        if(foundAllReferences)
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
    
    private void OnEnable()
    {
        TongueController.Instance.onChangeColor += RepaintRoulette;
    }

    private void OnDisable()
    {
        TongueController.Instance.onChangeColor -= RepaintRoulette;
    }
}
