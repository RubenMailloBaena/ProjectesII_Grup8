using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StrechEffect : IStrechEffect
{
    private Color effectColor;
    private ColorType colorType;
    private float stretchAmount;
    private float inverseStrechMultiplier;
    private LayerMask layerMask;
    
    private Color previousColor;
    private GameObject obstacle;
    private Vector3 initialScale;

    private bool doneRevertingEffect = false;
    private bool revertedColor = false;

    private float colXsize = 0.7f, colYsize = 2;
    private Collider2D[] colliders = new Collider2D[1];
    private RaycastHit2D[] hits = new RaycastHit2D[1];
        
    public StrechEffect(Color color, ColorType colorType, float multiplier, float inverseStrechMultiplier,LayerMask layerMask)
    {
        effectColor = color;
        this.colorType = colorType;
        stretchAmount = multiplier;
        this.layerMask = layerMask;
        this.inverseStrechMultiplier = inverseStrechMultiplier;
    }

    public void InitializeEffect(GameObject target)
    {
        obstacle = target;
        previousColor = target.GetComponent<SpriteRenderer>().color;
        target.GetComponent<SpriteRenderer>().color = effectColor;
        initialScale = target.GetComponent<ObstacleEffectLogic>().getInitialScale();
    }

    public void ApplyEffect()
    {
        Debug.Log(colliders.Length);
        if (colliders.Length == 1 || hits.Length == 1)
            StrechObject(false);

        //colliders = Physics2D.OverlapBoxAll(new Vector2(obstacle.transform.position.x, obstacle.transform.position.y + colYsize / 2), 
        //    new Vector2(obstacle.transform.localScale.x - colXsize, obstacle.transform.localScale.y - colYsize), 0, layerMask);

        //RaycastHit2D[] hits = Physics2D.RaycastAll(obstacle.transform.position, obstacle.transform.up, obstacle.transform.localScale.y / 2, layerMask);
        //if (hits.Length == 1) {
        //    StrechObject(false);
        //}
    }

    private void StrechObject(bool inverted) {
        int obstacleRotation = (int)obstacle.transform.eulerAngles.z;
        Vector2 collisionPosition = Vector2.zero;
        Vector2 collisionSize = Vector2.zero;

        switch (obstacleRotation) {

            case 90: //izquierda
                if(!inverted)
                    obstacle.transform.position = new Vector2(obstacle.transform.position.x - (stretchAmount / 2), obstacle.transform.position.y);
                else
                    obstacle.transform.position = new Vector2(obstacle.transform.position.x + (inverseStrechMultiplier / 2), obstacle.transform.position.y);

                hits = Physics2D.RaycastAll(obstacle.transform.position, obstacle.transform.up, obstacle.transform.localScale.y / 2, layerMask);
                colliders = new Collider2D[0];

                break;


            case 180: //abajo
                if (!inverted)
                    obstacle.transform.position = new Vector2(obstacle.transform.localPosition.x, obstacle.transform.localPosition.y - (stretchAmount / 2));
                else
                    obstacle.transform.position = new Vector2(obstacle.transform.localPosition.x, obstacle.transform.localPosition.y + (inverseStrechMultiplier / 2));

                collisionPosition = new Vector2(obstacle.transform.position.x, obstacle.transform.position.y - (colYsize / 2) - (stretchAmount / 2));
                collisionSize = new Vector2(obstacle.transform.localScale.x * colXsize, obstacle.transform.localScale.y - colYsize );
                colliders = Physics2D.OverlapBoxAll(collisionPosition, collisionSize, 0, layerMask);
                hits = new RaycastHit2D[0];

                break;


            case 270: //derecha
                if (!inverted)
                    obstacle.transform.position = new Vector2(obstacle.transform.position.x + (stretchAmount / 2), obstacle.transform.position.y);
                else
                    obstacle.transform.position = new Vector2(obstacle.transform.position.x - (inverseStrechMultiplier / 2), obstacle.transform.position.y);

                hits = Physics2D.RaycastAll(obstacle.transform.position, obstacle.transform.up, obstacle.transform.localScale.y / 2, layerMask);
                colliders = new Collider2D[0];

                break;


            default: //arriba
                if (!inverted)
                    obstacle.transform.position = new Vector2(obstacle.transform.localPosition.x, obstacle.transform.localPosition.y + (stretchAmount / 2));
                else
                    obstacle.transform.position = new Vector2(obstacle.transform.position.x, obstacle.transform.position.y - (inverseStrechMultiplier / 2));

                collisionPosition = new Vector2(obstacle.transform.position.x, obstacle.transform.position.y + (colYsize / 2) + (stretchAmount / 2));
                collisionSize = new Vector2(obstacle.transform.localScale.x * colXsize, obstacle.transform.localScale.y - colYsize );
                colliders = Physics2D.OverlapBoxAll(collisionPosition, collisionSize, 0, layerMask);
                hits = new RaycastHit2D[0];

                break;

        }

        if (!inverted)
            obstacle.transform.localScale = new Vector2(obstacle.transform.localScale.x, obstacle.transform.localScale.y + stretchAmount);
        else
            obstacle.transform.localScale = new Vector2(obstacle.transform.localScale.x, obstacle.transform.localScale.y - inverseStrechMultiplier);
    }

    public void RemoveEffect(GameObject target)
    {
        if (!revertedColor)
        {
            target.GetComponent<SpriteRenderer>().color = previousColor;
            revertedColor = true;
        }


        if (target.transform.localScale.y > initialScale.y)
        {
            StrechObject(true);
        }
        else {
            doneRevertingEffect = true;
        }
    }


    public bool getRevertingEffect() {
        return doneRevertingEffect;    
    }

    public ColorType getColorType()
    {
        return colorType;
    }

}
