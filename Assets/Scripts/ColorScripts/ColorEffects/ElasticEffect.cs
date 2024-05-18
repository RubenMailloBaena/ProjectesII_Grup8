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
        this.distanceMultiplier = distanceMultiplier;
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

       
        float verticalForce = minImpulse + (jumpHeight * hightMultiplier);
        
        if (verticalForce >= 150)
        {
            verticalForce = 70;
        }


        float horizontalForce = distanceMultiplier*100000;

        rb.velocity = new Vector2(horizontalForce, verticalForce);


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
    

