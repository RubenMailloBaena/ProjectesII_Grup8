using UnityEngine;
using System;
using System.Collections.Generic;


public class TongueController : MonoBehaviour
{
    //Singletone pattern
    private static TongueController instance;
    public static TongueController Instance {  
        get {
            if (instance == null)
                instance = FindAnyObjectByType<TongueController>();
            return instance;
        } }

    [Header("BLOQUEAR EFECTOS ESCENAS")]
    [SerializeField] private bool canUseStrech = true;
    [SerializeField] private bool canUseElastic = true;
    [SerializeField] private bool canUseWater = true;
    private ColorType[] colorTypes;
    private int colorIndex = 0;
    private ColorType lastColor;

    [Header("OTHER GAMEOBJECTS")]
    [SerializeField] private Transform tongueEnd;
    [SerializeField] private Transform tongueOrigin;

    [Header("TONGUE PARAMETERS")]
    [SerializeField] private float tongueSpeed;
    [SerializeField] private float maxTongueDistance;
    [SerializeField] private float detectionRadius;
    [SerializeField] private float maxAngleToShoot;
    [SerializeField] private float extraAngleToShoot;
    [SerializeField] private LayerMask tongueCanCollide;

    private bool shootTongue = false;
    private bool cancelShoot = false;
    private bool canShootAgain = true;
    private bool getDirectionAgain = true;
    private bool canCheckCollisions = true;
    private bool inWater = false;

    private Vector3 firstDirection;
    private Vector3 shootDirection;

    public event Action onShootingTongue;
    public event Action onNotMovingTongue;
    public event Action<ColorType> onPaintPlayer;

    private LineRenderer lineRenderer;
    private ColorManager colorManager;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;

        colorManager = FindAnyObjectByType<ColorManager>(); 

        SetColorsCanShoot();
    }

    private void SetColorsCanShoot() {
        List<ColorType> colorTypesList = new List<ColorType>();

        if (canUseElastic)
            colorTypesList.Add(ColorType.Elastic);
        if (canUseWater)
            colorTypesList.Add(ColorType.Water);
        if (canUseStrech)
            colorTypesList.Add(ColorType.Strech);

        colorTypes = colorTypesList.ToArray();

        lastColor = ColorType.Default;
    }

    private void FixedUpdate()
    {
        lineRenderer.SetPosition(0, tongueOrigin.position);
        lineRenderer.SetPosition(1, tongueEnd.position);

        ShootTongue();
        CheckTongueCollisions();
        CheckMaxTongueDistance();
        ChangePlayerColor(colorTypes[colorIndex]);
    }

    private void ShootTongue() {
        if (shootTongue) {
            GetShootingDirection();
            CheckIfShootingBack();
            if (!cancelShoot) { 
                tongueEnd.position += shootDirection * tongueSpeed * Time.fixedDeltaTime;
            }
        }
        else
        {
            if (Vector3.Distance(tongueOrigin.position, tongueEnd.position) != 0)
            {
                tongueEnd.position = Vector3.MoveTowards(tongueEnd.position, tongueOrigin.position, tongueSpeed*Time.fixedDeltaTime);
            }
            else    
            {
                PlayerAnimations.Instance.ChangeHeadAnimation(HeadAnim.Close);
                canShootAgain = true;
                cancelShoot = false;
                getDirectionAgain = true;
                canCheckCollisions = false;
                onNotMovingTongue?.Invoke();
            }
        }
    }

    private void GetShootingDirection() {
        if (getDirectionAgain)
        {
            Vector3 mousePositionWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            firstDirection = mousePositionWorld - tongueOrigin.transform.position;
            firstDirection.z = 0.0f;
            getDirectionAgain = false;
        }
        shootDirection = firstDirection.normalized;
    }

    private void CheckIfShootingBack() {
        Vector2 playerRight = transform.right;

        if (!CharacterMovement.Instance.GetFacingRight()) //mirando a la izquierda
            playerRight = -playerRight;

        float angle = Vector2.Angle(shootDirection, playerRight);

        if (angle > maxAngleToShoot) { //disparando a la espalda
            if (Vector2.Angle(shootDirection, transform.up) < extraAngleToShoot)
            {
                shootDirection = Vector2.up;
            }
            else if (Vector2.Angle(shootDirection, -transform.up) < extraAngleToShoot)
            {
                shootDirection = -Vector2.up;
            }
            else
            {
                shootTongue = false;
                cancelShoot = true;
            }
        }
    }

    private void CheckTongueCollisions() {
        if (canCheckCollisions) {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(tongueEnd.position, detectionRadius, tongueCanCollide);

            for (int i = 0; i < hitColliders.Length; i++) {
;               if (hitColliders[i].gameObject.tag.Equals("Door")) {
                    hitColliders[i].gameObject.GetComponent<NextLevelDoor>().DoorCollided();
                }
                else if (hitColliders[i].gameObject.tag.Equals("PaintableObstacle")) {
                    ChangeObjectEffect(hitColliders[i].gameObject);
                }
                shootTongue = false;
                canCheckCollisions = false;
            }
        }
    }


    private void ChangeObjectEffect(GameObject target) {
        IColorEffect currentEffect = colorManager.GetColorEffect(colorTypes[colorIndex]);
        target.GetComponent<ObstacleEffectLogic>().ApplyEffect(currentEffect);
    }


    private void CheckMaxTongueDistance()
    {
        float currentDistance = Vector3.Distance(tongueOrigin.position, tongueEnd.position);

        if (currentDistance >= maxTongueDistance) {
            shootTongue = false;
        }
    }



    private void ChangePlayerColor(ColorType colorType) {

        if (lastColor != colorType) {
            int counter = 0;
            while (colorManager.GetAssigneds(colorType))
            {
                SwapColor();
                counter++;
                Debug.Log(counter);
                if (counter >= colorTypes.Length)
                {
                    colorType = ColorType.Default;
                    break;
                }
            }
            Debug.Log("Tongue contreoller" + colorType);
            onPaintPlayer?.Invoke(colorType);
            lastColor = colorType;
        }
    }

    private void SwapColor() {
        colorIndex++;
        if (colorIndex > colorTypes.Length - 1)
            colorIndex = 0;
    }

    private void setShootTongue() {
        if (canShootAgain && !inWater) {
            PlayerAnimations.Instance.ChangeHeadAnimation(HeadAnim.Open);
            shootTongue = true;
            canShootAgain = false;
            canCheckCollisions = true;
            onShootingTongue?.Invoke();
            
        }
    }

    private void InWater() {
        inWater = !inWater;
    }

    public float GetMaxAngleToShoot() {
        return maxAngleToShoot;
    }

    public float GetExtraAngleToShoot() {
        return extraAngleToShoot;
    }

    public Vector3 GetShootDirection() {
        return shootDirection;
    }

    private void OnEnable()
    {
        PlayerInputs.Instance.onShoot += setShootTongue;
        PlayerInputs.Instance.onSwapColor += SwapColor;
        WaterEffect.onWater += InWater;
    }

    private void OnDisable()
    {
        PlayerInputs.Instance.onShoot -= setShootTongue;
        PlayerInputs.Instance.onSwapColor -= SwapColor;
        WaterEffect.onWater -= InWater;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.right * 3);

        Gizmos.DrawWireSphere(tongueEnd.position, detectionRadius);
    }
}
