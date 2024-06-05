using UnityEngine;
using System;
using Unity.VisualScripting;

public class ObstacleEffectLogic : MonoBehaviour
{
    public static ObstacleEffectLogic instance;

    [SerializeField] private float maxPlayerDistance;

    private IColorEffect currentColorEffect;
    private ColorType currentColorType;
    private GameObject player;

    [SerializeField] private SpriteRenderer PlatformSprite;
    [SerializeField] private SpriteRenderer PlatformColumn;

    public static event Action<ColorType> onChangeEffect;

    //LOGICA STRECH
    private IStrechEffect lastStrechEffect;
    private Vector3 initialScale;

    public void ApplyEffect(IColorEffect colorEffect)
    {
        if (currentColorType == ColorType.Default)
        {
            currentColorEffect = colorEffect;
            currentColorEffect.InitializeEffect(gameObject);
            currentColorType = currentColorEffect.getColorType();
        }
        else
        {
            if (currentColorType == ColorType.Strech)
            {
                //STRECH LOGIC
                IStrechEffect effect = currentColorEffect as IStrechEffect;
                lastStrechEffect = effect;
            }

            if (colorEffect.getColorType() != ColorType.Default)
            {
                onChangeEffect?.Invoke(colorEffect.getColorType());
            }

            onChangeEffect?.Invoke(currentColorType);
            currentColorEffect.RemoveEffect(gameObject);
            currentColorEffect = null;
            currentColorType = ColorType.Default;
        }
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
        
        currentColorType = ColorType.Default;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        initialScale = transform.localScale;

        PlatformColumn.size = new Vector2(PlatformSprite.size.x, PlatformSprite.size.y);

        player = GameObject.Find("TongueOrigin"); 
    }

    void FixedUpdate()
    {
        StrechEffect();
        RevertStrechEffect();
        CheckPlayerDistance();
    }

    private void CheckPlayerDistance()
    {
        //Si el jugador esta muy lejos, devolvemos el color
        if (currentColorType != ColorType.Default)
        {
            if (Vector3.Distance(transform.position, player.transform.position) >= maxPlayerDistance)
            {
                RemoveAllEffects(ColorType.Default);
            }
        }
    }

    //STRECH LOGIC

    private void StrechEffect()
    {
        if(PlatformColumn.size != PlatformSprite.size)
            PlatformColumn.size = new Vector2(PlatformSprite.size.x, PlatformSprite.size.y);
        if (currentColorType == ColorType.Strech)
        {
            IStrechEffect effect = currentColorEffect as IStrechEffect;
            effect.ApplyEffect();
            PlatformColumn.size = new Vector2(PlatformSprite.size.x, PlatformSprite.size.y);
        }
    }

    private void RevertStrechEffect()
    {
        if (lastStrechEffect != null && currentColorType != ColorType.Strech)
        {
            if (!lastStrechEffect.getRevertingEffect())
            {
                lastStrechEffect.RemoveEffect(gameObject);
            }
            else
            {
                lastStrechEffect = null;
            }
        }
    }

    //ELASTIC LOGIC
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (currentColorEffect != null)
        {
            if (currentColorType == ColorType.Elastic && collision.gameObject.CompareTag("Player"))
            {
                IElasticEffect effect = currentColorEffect as IElasticEffect;
                effect.ApplyEffect(collision.gameObject);
            }
        }
    }

    //WATER LOGIC
    private void OnTriggerEnter2D(Collider2D other) {
            ApplyWaterEffect(other, true);
    }

    private void OnTriggerStay2D(Collider2D other) {
            ApplyWaterEffect(other, true);
    }

    private void OnTriggerExit2D(Collider2D other) {
        ApplyWaterEffect(other, false);
    }

    private void ApplyWaterEffect(Collider2D other, bool entering)
    {
        if (currentColorEffect != null) {
            if (currentColorType == ColorType.Water && other.gameObject.CompareTag("Player")) {
                IWaterEffect effect = currentColorEffect as IWaterEffect;
                effect.ApplyEffect(entering);
            }
        }
    }


    public Vector3 getInitialScale()
    {
        return initialScale;
    }

    public ColorType getCurrentColorType()
    {
        return currentColorType;
    }

    public bool LastColorTypeWasStrech() {
        if (lastStrechEffect != null && gameObject.transform.parent.transform.parent.eulerAngles.z == 0)
            return true;
        return false;
    }


    private void RemoveAllEffects(ColorType colorType)
    {
        ApplyEffect(FindAnyObjectByType<ColorManager>().GetColorEffect(colorType));
    }

    private void OnDrawGizmosSelected()
    {
        if (currentColorType != ColorType.Default && player != null)
        {
            Gizmos.color = Color.black;
            Vector3 direction = (player.transform.position - transform.position).normalized;
            Gizmos.DrawLine(transform.position, transform.position + direction * maxPlayerDistance);
        }

        if (currentColorType == ColorType.Water)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawCube(transform.position, transform.localScale);
        }
    }


    private void OnEnable()
    {
        GameManager.instance.onPlayerDeath += RemoveAllEffects;
    }

    private void OnDisable()
    {
        GameManager.instance.onPlayerDeath -= RemoveAllEffects;
    }
}
