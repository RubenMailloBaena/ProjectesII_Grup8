using UnityEngine;
using System;
using Unity.VisualScripting;

public class ObstacleEffectLogic : MonoBehaviour
{
    private static ObstacleEffectLogic instance;

    public static ObstacleEffectLogic Instace
    {
        get
        {
            if (instance == null)
                instance = FindAnyObjectByType<ObstacleEffectLogic>();
            return instance;
        }
    }

    [SerializeField] private float maxPlayerDistance;

    private IColorEffect currentColorEffect;
    private ColorType currentColorType;
    private GameObject player;

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
        currentColorType = ColorType.Default;
        initialScale = transform.localScale;

        player = GameObject.Find("Player");
        Debug.Log(player); 
    }

    void FixedUpdate()
    {
        StrechEffect();
        RevertStrechEffect();

        CheckPlayerDistance();

        WaterEffect();
    }

    private void CheckPlayerDistance()
    {
        //Si el jugador esta muy lejos, devolvemos el color
        if (currentColorType != ColorType.Default)
        {
            //Debug.Log( player.transform.position);
            //if (Vector3.Distance(transform.position, player.transform.position) >= maxPlayerDistance)
            //{
            //    RemoveAllEffects(ColorType.Default);
            //}
        }
    }

    //STRECH LOGIC

    private void StrechEffect()
    {
        if (currentColorType == ColorType.Strech)
        {
            IStrechEffect effect = currentColorEffect as IStrechEffect;
            effect.ApplyEffect();
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
    private void WaterEffect() {
        if (currentColorEffect != null) {
            if (currentColorType == ColorType.Water) {
                IWaterEffect effect = currentColorEffect as IWaterEffect;
                effect.ApplyEffect();
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

    private void RemoveAllEffects(ColorType colorType)
    {
        ApplyEffect(FindAnyObjectByType<ColorManager>().GetColorEffect(colorType));
    }

    private void OnDrawGizmos()
    {
        if (currentColorType != ColorType.Default && player != null)
        {
            Gizmos.color = Color.black;
            Vector3 direction = (player.transform.position - transform.position).normalized;
            Gizmos.DrawLine(transform.position, transform.position + direction * maxPlayerDistance);
        }
    }


    private void OnEnable()
    {
        GameManager.Instance.onPlayerDeath += RemoveAllEffects;
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.onPlayerDeath -= RemoveAllEffects;
    }
}
