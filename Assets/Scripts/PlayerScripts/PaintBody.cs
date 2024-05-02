using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class PaintBody : MonoBehaviour
{
    [Header("BODY PARTS TO PAINT")] //temporal hasta usar el shader
    [SerializeField] private GameObject HeadSprite;
    [SerializeField] private GameObject bodySpritesParent;
    private int bodyChildCount;

    private Color lastColor = Color.black;

    private void PaintPlayer(Color color) {
        
        if(color != lastColor){
            ChangeColors(HeadSprite, color);
            ChangeColors(bodySpritesParent, color);
            lastColor = color;
        }
        
    }
    
    private void ChangeColors(GameObject target, Color color) {
        bodyChildCount = target.transform.childCount;
        for (int i = 0; i < bodyChildCount; i++)
        {
            Transform child = target.transform.GetChild(i);
            SpriteRenderer childRender = child.GetComponent<SpriteRenderer>();

            if (childRender != null)
                childRender.material.color = color;
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