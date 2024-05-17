using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

public class ElasticEffect : IElasticEffect
{
    private Color effectColor;
    private ColorType colorType;

    private float minImpulse;
    private float hightMultiplier;
    private float distanceMultiplier;

    private Color previousColor;
    private GameObject obstacle;

    public ElasticEffect(Color color, ColorType colorType, float minImpulse, float hightMultiplier, float distanceMultiplier)
    {
        effectColor = color;
        this.colorType = colorType;
        this.minImpulse = minImpulse;
        this.hightMultiplier = hightMultiplier;
        this.distanceMultiplier = this.distanceMultiplier;
    }


    public void InitializeEffect(GameObject target)
    {
        Debug.Log("inside " + target.gameObject.name);
        obstacle = target;
        previousColor = target.GetComponent<SpriteRenderer>().color;
        target.GetComponent<SpriteRenderer>().color = effectColor;
    }

    public void ApplyEffect(GameObject player)
    {
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

        float jumpHeight = Mathf.Abs(player.GetComponent<CharacterMovement>().getLastJumpPosition().y 
            - (obstacle.transform.position.y + obstacle.transform.localScale.y / 2));

        
        float totalForce = minImpulse + (jumpHeight * hightMultiplier);
        
        if (totalForce >= 150)
        {
            totalForce = 70;
        }

        rb.velocity = new Vector2(distanceMultiplier*100, totalForce);

       // rb.velocity = player.transform.up * totalForce;


    }

    public void RemoveEffect(GameObject target)
    {
        target.GetComponent<SpriteRenderer>().color = previousColor;
    }

    public ColorType getColorType()
    {
        return colorType;
    }
}
    

