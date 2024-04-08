using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using UnityEngine.UIElements;

public class TongueController : MonoBehaviour
{
    [SerializeField] private Transform tongueEnd;
    [SerializeField] private Transform tongueOrigin;

    [SerializeField] private float tongueSpeed;
    [SerializeField] private float maxTongueDistance;
    [SerializeField] private float detectionRadius;

    private ColorType[] colorTypes;
    private int currentColorIndex = 0;

    private bool shootTongue = false;
    private bool canShootAgain = true;
    private bool getDirectionAgain = true;

    private Vector3 firstDirection;
    private bool pointingUp = false;
    private bool pointingDown = false;
    private bool pointingStraight = false;

    public static event Action onShootingTonge;

    private LineRenderer lineRenderer;
    private ColorManager colorManager;

    private void Start()
    {
        colorTypes = new ColorType[] {
            ColorType.Elastic,
            ColorType.Water,
            ColorType.Strech
        };

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;

        colorManager = FindAnyObjectByType<ColorManager>(); 
    }

    private void FixedUpdate()
    {
        //assignar posiciones al line renderer
        lineRenderer.SetPosition(0, tongueOrigin.position);
        lineRenderer.SetPosition(1, tongueEnd.position);

        ShootTongue();
        CheckTongueCollisions();
        CheckMaxTongueDistance();
    }

    private void ShootTongue() {
        if (shootTongue) {
            Debug.Log("Shooting tongue");
            Vector3 shootDirection = GetShootingDirection();
            tongueEnd.position += shootDirection * tongueSpeed;
        }
        else
        {
            if (Vector3.Distance(tongueOrigin.position, tongueEnd.position) != 0)
            {
                tongueEnd.position = Vector3.MoveTowards(tongueEnd.position, tongueOrigin.position, tongueSpeed);
            }
            else 
            {
                canShootAgain = true;
                getDirectionAgain = true;
            }
        }
    }

    private Vector3 GetShootingDirection() {
        if (getDirectionAgain) {
            CharacterMovement characterMovement = GetComponentInParent<CharacterMovement>() ?? GetComponent<CharacterMovement>();

            // Inicializa la dirección con el vector nulo.
            Vector3 direction = Vector3.zero;

            // Determina la dirección basada en las entradas verticales.
            if (pointingUp) {
                direction += Vector3.up;
            } else if (pointingDown) {
                direction += Vector3.down;
            }

            // Añade la dirección horizontal solo si pointingStraight está activo.
            // Esto permite disparos directamente hacia arriba o abajo sin inclinarse hacia los lados.
            if (pointingStraight) {
                Vector3 horizontalDirection = characterMovement.FacingRight ? Vector3.right : Vector3.left;
                direction += horizontalDirection;
            }

            // Si no se detectó ninguna entrada (o solo pointingStraight sin up/down),
            // entonces dispara en la dirección horizontal basada en hacia dónde está mirando el personaje.
            if (direction == Vector3.zero || (pointingStraight && !pointingUp && !pointingDown)) {
                Vector3 horizontalDirection = characterMovement.FacingRight ? Vector3.right : Vector3.left;
                direction = horizontalDirection;
            }

            firstDirection = direction.normalized;
            getDirectionAgain = false;
        }

        return firstDirection;
    }






    private void CheckTongueCollisions() {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(tongueEnd.position, detectionRadius);

        for (int i = 0; i < hitColliders.Length; i++) {
            if (hitColliders[i].gameObject.tag.Equals("Obstacle")) {
                shootTongue = false;
                break;
            }
            else if (hitColliders[i].gameObject.tag.Equals("PaintableObstacle")) {
                shootTongue = false;
                ChangeObjectEffect(hitColliders[i].gameObject);
            }
        }
    }


    private void ChangeObjectEffect(GameObject target) {
        IColorEffect currentEffect = colorManager.GetColorEffect(colorTypes[currentColorIndex]);
        ObstacleEffectLogic colorableObject = target.GetComponent<ObstacleEffectLogic>();
        colorableObject.ApplyEffect(currentEffect);
    }


    private void CheckMaxTongueDistance()
    {
        float currentDistance = Vector3.Distance(tongueOrigin.position, tongueEnd.position);

        if (currentDistance >= maxTongueDistance || currentDistance <= -maxTongueDistance) {
            shootTongue = false;
        }
    }


    private void setShootTongue() {
        if (canShootAgain) { 
            shootTongue = true;
            canShootAgain = false;
            onShootingTonge?.Invoke();
        }
    }

    private void setPointingUp(){
        Debug.Log("W pressed");
        pointingUp = !pointingUp;
        Debug.Log(pointingUp);
    }

    private void setPointingDown(){
        Debug.Log("S pressed");
        pointingDown = !pointingDown;
    }

    private void setPointingStraight(){
        pointingStraight = !pointingStraight;
    }



    private void OnEnable()
    {
        PlayerInputs.onShoot += setShootTongue;
        PlayerInputs.onShootUp += setPointingUp;
        PlayerInputs.onShootDown += setPointingDown;
        PlayerInputs.onShootStraight += setPointingStraight;
    }

    private void OnDisable()
    {
        PlayerInputs.onShoot -= setShootTongue;
        PlayerInputs.onShootUp -= setPointingUp;
        PlayerInputs.onShootDown -= setPointingDown;
        PlayerInputs.onShootStraight -= setPointingStraight;
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.right * 3);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(tongueEnd.position, detectionRadius);
    }
}
