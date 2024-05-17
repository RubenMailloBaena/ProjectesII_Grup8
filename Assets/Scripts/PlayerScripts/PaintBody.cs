using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class PaintBody : MonoBehaviour
{
    [System.Serializable]
    public class BodyPartsSpriteRenderer
    {
        public SpriteRenderer m_Cap;
        public SpriteRenderer m_Capa16;
        public SpriteRenderer m_Fondoulls;
        public SpriteRenderer m_NinaD;
        public SpriteRenderer m_NinaEsq;
        public SpriteRenderer m_parpD;
        public SpriteRenderer m_parpE;
        public SpriteRenderer m_Capa3;
        public SpriteRenderer m_Capa4;
        public SpriteRenderer m_Capa5;
        public SpriteRenderer m_Capa6;
        public SpriteRenderer m_Capa8;
    }
    [System.Serializable]
    public class BodyPartsSprites
    {
        public Sprite m_Cap;
        public Sprite m_Capa16;
        public Sprite m_Fondoulls;
        public Sprite m_NinaD;
        public Sprite m_NinaEsq;
        public Sprite m_parpD;
        public Sprite m_parpE;
        public Sprite m_Capa3;
        public Sprite m_Capa4;
        public Sprite m_Capa5;
        public Sprite m_Capa6;
        public Sprite m_Capa8;
    }

    [SerializeField] private BodyPartsSpriteRenderer m_BodyPartsSpriteRenderer;
    [Header("BODY PARTS TO PAINT")] //temporal hasta usar el shader
    [SerializeField] private GameObject currentHeadSprite;
    [SerializeField] private GameObject currentBodySprite;

    [Header("STRECH SPRITES")]
    [SerializeField] private GameObject strechtHead;
    [SerializeField] private GameObject strechBody;
    [SerializeField] private BodyPartsSprites m_StrechBodyPartsSprites;

    [Header("BOUNCE SPRITES")]
    [SerializeField] private GameObject bounceHead;
    [SerializeField] private GameObject bounceBody;
    [SerializeField] private BodyPartsSprites m_BounceBodyPartsSprites;

    [Header("WATER SPRITES")]
    [SerializeField] private GameObject waterHead;
    [SerializeField] private GameObject waterBody;
    [SerializeField] private BodyPartsSprites m_WaterBodyPartsSprites;

    //[Header("DEFAULT SPRITES")]
    private GameObject defaultHead;
    private GameObject defaultBody;
    [SerializeField] private BodyPartsSprites m_DefaultBodyPartsSprites;

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
                    Debug.Log("Water");
                    ChangeColors(m_WaterBodyPartsSprites);
                    currentHeadSprite = waterHead;
                    currentBodySprite = waterBody;
                    /*ChangeColors(currentHeadSprite, waterHead);
                    ChangeColors(currentBodySprite, waterBody);*/
                    break;

                case ColorType.Elastic:
                    Debug.Log("Elastic");
                    ChangeColors(m_BounceBodyPartsSprites);
                    currentHeadSprite = bounceHead;
                    currentBodySprite = bounceBody;
                    /*ChangeColors(currentHeadSprite, bounceHead);
                    ChangeColors(currentBodySprite, bounceBody);*/
                    break;

                case ColorType.Strech:
                    Debug.Log("Strech");
                    ChangeColors(m_StrechBodyPartsSprites);
                    currentHeadSprite = strechtHead;
                    currentBodySprite = strechBody;
                    /*ChangeColors(currentHeadSprite, strechtHead);
                    ChangeColors(currentBodySprite, strechBody);*/
                    break;

                default:
                    Debug.Log("Default");
                    ChangeColors(m_DefaultBodyPartsSprites);
                    currentHeadSprite = defaultHead;
                    currentBodySprite = defaultBody;
                    /*ChangeColors(currentHeadSprite, defaultHead);
                    ChangeColors(currentBodySprite, defaultBody);*/
                    break;
            }

            lastColor = color;
        }
    }

    private void ChangeColors(BodyPartsSprites _BodyPartsSprites)
    {
        m_BodyPartsSpriteRenderer.m_Cap.sprite = _BodyPartsSprites.m_Cap;
        m_BodyPartsSpriteRenderer.m_Capa16.sprite = _BodyPartsSprites.m_Capa16;
        m_BodyPartsSpriteRenderer.m_Fondoulls.sprite = _BodyPartsSprites.m_Fondoulls;
        m_BodyPartsSpriteRenderer.m_NinaD.sprite = _BodyPartsSprites.m_NinaD;
        m_BodyPartsSpriteRenderer.m_NinaEsq.sprite = _BodyPartsSprites.m_NinaEsq;
        m_BodyPartsSpriteRenderer.m_parpD.sprite = _BodyPartsSprites.m_parpD;
        m_BodyPartsSpriteRenderer.m_parpE.sprite = _BodyPartsSprites.m_parpE;


        m_BodyPartsSpriteRenderer.m_Capa3.sprite = _BodyPartsSprites.m_Capa3;
        m_BodyPartsSpriteRenderer.m_Capa4.sprite = _BodyPartsSprites.m_Capa4;
        m_BodyPartsSpriteRenderer.m_Capa5.sprite = _BodyPartsSprites.m_Capa5;
        m_BodyPartsSpriteRenderer.m_Capa6.sprite = _BodyPartsSprites.m_Capa6;
        m_BodyPartsSpriteRenderer.m_Capa8.sprite = _BodyPartsSprites.m_Capa8;

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