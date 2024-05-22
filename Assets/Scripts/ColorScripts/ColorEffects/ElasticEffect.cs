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
    private SpriteRenderer colorChangePart;

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
        GetAllObstacleReferences(target);
        previousColor = colorChangePart.color;
        colorChangePart.color = effectColor;
    }

    private void GetAllObstacleReferences(GameObject target)
    {
        obstacle = target;
        colorChangePart = obstacle.transform.parent.transform.Find("ColorChange").GetComponent<SpriteRenderer>();
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


        float horizontalForce = distanceMultiplier;

        rb.velocity = new Vector2(horizontalForce, verticalForce);


    }

    public void RemoveEffect(GameObject target)
    {
        colorChangePart.color = previousColor;
    }

    public ColorType getColorType()
    {
        return colorType;
    }
}
    

