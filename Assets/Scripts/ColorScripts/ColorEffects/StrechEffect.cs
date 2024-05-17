using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StrechEffect : IStrechEffect
{
    //logic variables
    private Color effectColor;
    private ColorType colorType;
    private float stretchAmount;
    private float inverseStrechMultiplier;
    private float extendSpriteVelocity;
    private LayerMask layerMask;
    private Color previousColor;
    private Vector3 initialScale;

    //Objectes del obstacle
    private GameObject currentObstacle; //obstacle actual
    private GameObject parentObject;
    private GameObject movingPart;
    private GameObject topSprite; //la part de d'alt de la plataforma
    private GameObject extendablePart; //part extensible de la plataforma
    private SpriteRenderer extendableSprite;

    //control logic
    private bool doneRevertingEffect = false;
    private bool revertedColor = false;

    private float colXsize = 0.7f;
    private float colYsize = 2;
    private Collider2D[] colliders = new Collider2D[1];
    private RaycastHit2D[] hits = new RaycastHit2D[1];

    public StrechEffect(Color color, ColorType colorType, float multiplier, float inverseStrechMultiplier, float extendSpriteVelocity, float colXsize, LayerMask layerMask)
    {
        effectColor = color;
        this.colorType = colorType;
        stretchAmount = multiplier;
        this.layerMask = layerMask;
        this.inverseStrechMultiplier = inverseStrechMultiplier;
        this.colXsize = colXsize;
        this.extendSpriteVelocity = extendSpriteVelocity;
    }

    public void InitializeEffect(GameObject target)
    {
        getAllObstacleObjects(target);
        previousColor = target.GetComponent<SpriteRenderer>().color;
        target.GetComponent<SpriteRenderer>().color = effectColor;
        initialScale = target.GetComponent<ObstacleEffectLogic>().getInitialScale();
    }

    private void getAllObstacleObjects(GameObject currentObstacle)
    {
        this.currentObstacle = currentObstacle;
        parentObject = currentObstacle.transform.parent.transform.parent.gameObject;
        topSprite = parentObject.transform.Find("Sprite Holder/TopSprite").gameObject;
        movingPart = parentObject.transform.Find("MovingObstacle").gameObject;
        extendablePart = parentObject.transform.Find("Sprite Holder/ExtendablePart").gameObject;
        extendableSprite = extendablePart.GetComponent<SpriteRenderer>();
    }

    public void ApplyEffect()
    {
        if (colliders.Length == 1 || hits.Length == 1)
            StrechObject(false);
    }

    private void StrechObject(bool inverted)
    {
        int obstacleRotation = (int)parentObject.transform.eulerAngles.z;
        Vector2 collisionPosition = Vector2.zero;
        Vector2 collisionSize = Vector2.zero;

        switch (obstacleRotation)
        {

            case 90: //izquierda
                //CURRENT OBSTACLE
                if (!inverted)
                {
                    movingPart.transform.position = new Vector2(movingPart.transform.position.x - (stretchAmount / 2), movingPart.transform.position.y);

                    //sprite
                    extendablePart.transform.position = new Vector2(extendablePart.transform.position.x - (stretchAmount / 2), extendablePart.transform.position.y);
                    topSprite.transform.position = new Vector2(topSprite.transform.position.x - stretchAmount, topSprite.transform.position.y);
                }
                else
                {
                    movingPart.transform.position = new Vector2(movingPart.transform.position.x + (inverseStrechMultiplier / 2), movingPart.transform.position.y);

                    //sprite
                    extendablePart.transform.position = new Vector2(extendablePart.transform.position.x + (inverseStrechMultiplier / 2), extendablePart.transform.position.y);
                    topSprite.transform.position = new Vector2(topSprite.transform.position.x + inverseStrechMultiplier, topSprite.transform.position.y);
                }

                //COLLIDER TO STOP
                hits = Physics2D.RaycastAll(currentObstacle.transform.position, currentObstacle.transform.up, currentObstacle.transform.localScale.y / 2, layerMask);
                colliders = new Collider2D[0];

                break;


            case 180: //abajo
                //CURRENT OBSTACLE
                if (!inverted)
                {
                    movingPart.transform.position = new Vector2(movingPart.transform.position.x, movingPart.transform.position.y - (stretchAmount / 2));

                    //sprite
                    extendablePart.transform.position = new Vector2(extendablePart.transform.position.x, extendablePart.transform.position.y - (stretchAmount / 2));
                    topSprite.transform.position = new Vector2(topSprite.transform.position.x, topSprite.transform.position.y - stretchAmount);
                }
                else
                {
                    movingPart.transform.position = new Vector2(movingPart.transform.position.x, movingPart.transform.position.y + (inverseStrechMultiplier / 2));

                    //sprite
                    extendablePart.transform.position = new Vector2(extendablePart.transform.position.x, extendablePart.transform.position.y + (inverseStrechMultiplier / 2));
                    topSprite.transform.position = new Vector2(topSprite.transform.position.x, topSprite.transform.position.y + inverseStrechMultiplier);
                }

                //COLLIDER TO STOP
                collisionPosition = new Vector2(currentObstacle.transform.position.x, currentObstacle.transform.position.y - (colYsize / 2) - (stretchAmount / 2));
                collisionSize = new Vector2(currentObstacle.transform.localScale.x * colXsize, currentObstacle.transform.localScale.y - colYsize);
                colliders = Physics2D.OverlapBoxAll(collisionPosition, collisionSize, 0, layerMask);
                hits = new RaycastHit2D[0];

                break;


            case 270: //derecha
                //CURRENT OBSTACLE
                if (!inverted)
                {
                    movingPart.transform.position = new Vector2(movingPart.transform.position.x + (stretchAmount / 2), movingPart.transform.position.y);

                    //sprite
                    extendablePart.transform.position = new Vector2(extendablePart.transform.position.x + (stretchAmount / 2), extendablePart.transform.position.y);
                    topSprite.transform.position = new Vector2(topSprite.transform.position.x + stretchAmount, topSprite.transform.position.y);
                }
                else
                {
                    movingPart.transform.position = new Vector2(movingPart.transform.position.x - (inverseStrechMultiplier / 2), movingPart.transform.position.y);

                    //sprite
                    extendablePart.transform.position = new Vector2(extendablePart.transform.position.x - (inverseStrechMultiplier / 2), extendablePart.transform.position.y);
                    topSprite.transform.position = new Vector2(topSprite.transform.position.x - inverseStrechMultiplier, topSprite.transform.position.y);
                }

                //COLLIDER TO STOP
                hits = Physics2D.RaycastAll(currentObstacle.transform.position, currentObstacle.transform.up, currentObstacle.transform.localScale.y / 2, layerMask);
                colliders = new Collider2D[0];

                break;

            default: //arriba
                //CURRENT OBSTACLE
                if (!inverted)
                {
                    movingPart.transform.position = new Vector2(movingPart.transform.position.x, movingPart.transform.position.y + (stretchAmount / 2));

                    //sprite
                    extendablePart.transform.position = new Vector2(extendablePart.transform.position.x, extendablePart.transform.position.y + (stretchAmount / 2));
                    topSprite.transform.position = new Vector2(topSprite.transform.position.x, topSprite.transform.position.y + stretchAmount);
                }
                else
                {
                    movingPart.transform.position = new Vector2(movingPart.transform.position.x, movingPart.transform.position.y - (inverseStrechMultiplier / 2));

                    //sprite
                    extendablePart.transform.position = new Vector2(extendablePart.transform.position.x, extendablePart.transform.position.y - (inverseStrechMultiplier / 2));
                    topSprite.transform.position = new Vector2(topSprite.transform.position.x, topSprite.transform.position.y - inverseStrechMultiplier);
                }

                //COLLIDER TO STOP
                collisionPosition = new Vector2(currentObstacle.transform.position.x, currentObstacle.transform.position.y + (colYsize / 2) + (stretchAmount / 2));
                collisionSize = new Vector2(currentObstacle.transform.localScale.x * colXsize, currentObstacle.transform.localScale.y - colYsize);
                colliders = Physics2D.OverlapBoxAll(collisionPosition, collisionSize, 0, layerMask);
                hits = new RaycastHit2D[0];

                break;

        }

        if (!inverted)
        {
            currentObstacle.transform.localScale = new Vector2(currentObstacle.transform.localScale.x, currentObstacle.transform.localScale.y + stretchAmount);

            extendableSprite.size = new Vector2(extendableSprite.size.x, extendableSprite.size.y + extendSpriteVelocity);
        }
        else
        {
            currentObstacle.transform.localScale = new Vector2(currentObstacle.transform.localScale.x, currentObstacle.transform.localScale.y - inverseStrechMultiplier);

            extendableSprite.size = new Vector2(extendableSprite.size.x, extendableSprite.size.y - (extendSpriteVelocity / 2));
        }
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
        else
        {
            doneRevertingEffect = true;
        }
    }


    public bool getRevertingEffect()
    {
        return doneRevertingEffect;
    }

    public ColorType getColorType()
    {
        return colorType;
    }

}
