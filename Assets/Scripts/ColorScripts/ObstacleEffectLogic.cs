using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.Rendering;

public class ObstacleEffectLogic : MonoBehaviour
{
    private IColorEffect currentColorEffect;

    public void ApplyEffect(IColorEffect colorEffect) {
        if (currentColorEffect != null)
        {
            currentColorEffect = currentColorEffect.RemoveEffect(gameObject);
        }
        else 
        {
            currentColorEffect = colorEffect;
            currentColorEffect.InitializeEffect(gameObject);
        }
    }
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (currentColorEffect != null)
            if (currentColorEffect.getColorType() == ColorType.Elastic && collision.gameObject.CompareTag("Player")) {
            
                currentColorEffect.ApplyEffect(collision.gameObject);
        }
    }
}
