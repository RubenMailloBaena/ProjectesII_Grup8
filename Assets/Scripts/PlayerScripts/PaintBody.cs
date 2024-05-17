using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class PaintBody : MonoBehaviour
{
    [Header("BODY PARTS TO PAINT")] //temporal hasta usar el shader
    [SerializeField] private GameObject currentHeadSprite;
    [SerializeField] private GameObject currentBodySprite;

    [Header("STRECH SPRITES")]
    [SerializeField] private GameObject strechtHead;
    [SerializeField] private GameObject strechBody;

    [Header("BOUNCE SPRITES")]
    [SerializeField] private GameObject bounceHead;
    [SerializeField] private GameObject bounceBody;

    //[Header("DEFAULT SPRITES")]
    private GameObject defaultHead;
    private GameObject defaultBody;

    private int bodyChildCount;

    private ColorType lastColor = ColorType.Default;

    private void Awake()
    {
        defaultHead = currentHeadSprite;
        defaultBody = currentBodySprite;
    }

    private void PaintPlayer(ColorType color) {
        
        if (color != lastColor)
        {
            switch (color)
            {
                case ColorType.Water:
                    break;

                case ColorType.Elastic:
                    Debug.Log("Elastic");
                    ChangeColors(currentHeadSprite, bounceHead);
                    ChangeColors(currentBodySprite, bounceBody);
                    currentHeadSprite = bounceHead;
                    currentBodySprite = bounceBody;
                    break;

                case ColorType.Strech:
                    Debug.Log("Strech");
                    ChangeColors(currentHeadSprite, strechtHead);
                    ChangeColors(currentBodySprite, strechBody);
                    currentHeadSprite = strechtHead;
                    currentBodySprite = strechBody;
                    break;

                default:
                    Debug.Log("Default");
                    ChangeColors(currentHeadSprite, defaultHead);
                    ChangeColors(currentBodySprite, defaultBody);
                    currentHeadSprite = defaultHead;
                    currentBodySprite = defaultBody;
                    break;
            }

            lastColor = color;
        }
    }
    
    private void ChangeColors(GameObject target, GameObject changeSprite) {
        bodyChildCount = target.transform.childCount;
        for (int i = 0; i < bodyChildCount; i++)
        {
            Debug.Log(i);

            SpriteRenderer currentChild = target.transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>();
            SpriteRenderer changeChild = changeSprite.transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>();

            if (currentChild != null && changeChild != null)
                currentChild.sprite = changeChild.sprite;
        }
    }

    private void OnEnable()
    {
        TongueController.Instance.onPaintPlayer += PaintPlayer;
    }

    private void OnDisable()
    {
        TongueController.Instance.onPaintPlayer -= PaintPlayer;
    }
}