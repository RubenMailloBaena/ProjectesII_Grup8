using UnityEngine;
using System;
using System.Collections.Generic;


public class TongueController : MonoBehaviour
{
    //Singletone pattern
    private static TongueController instance;
    public static TongueController Instance {  
        get {
            if (instance is null)
                instance = FindAnyObjectByType<TongueController>();
            return instance;
        } }

    [Header("BLOQUEAR EFECTOS ESCENAS")]
    [SerializeField] private bool canUseStrech = true;
    [SerializeField] private bool canUseElastic = true;
    [SerializeField] private bool canUseWater = true;
    private ColorType[] colorTypes;
    private int colorIndex = 0;

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

    private bool shootTongue;
    private bool cancelShoot;
    private bool canShootAgain = true;
    private bool lastColorChangeRight = true;
    private bool getDirectionAgain = true;
    private bool canCheckCollisions = true;
    private bool inWater;
    private bool soundPlayed;

    private Vector3 firstDirection;
    private Vector3 shootDirection;

    public event Action onShootingTongue;
    public event Action onNotMovingTongue;
    public event Action<ColorType> onPaintPlayer;
    public event Action<int> onChangeColor;
    
    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;

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
                if (!soundPlayed)
                {
                    GameSoundEffects.Instance.PlayerSoundEffect(playerSounds.ShootTongue);
                    soundPlayed = true;
                }
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
                soundPlayed = false;
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
                GameSoundEffects.Instance.PlayerSoundEffect(playerSounds.TongueWrongDirection);
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
                else if (hitColliders[i].gameObject.tag.Equals("PaintableObstacle") && !inWater) {
                    ChangeObjectEffect(hitColliders[i].gameObject);
                }
                shootTongue = false;
                canCheckCollisions = false;
            }
        }
    }
    
    private void ChangeObjectEffect(GameObject target) {
        IColorEffect currentEffect = ColorManager.Instace.GetColorEffect(colorTypes[colorIndex]);
        target.GetComponent<ObstacleEffectLogic>().ApplyEffect(currentEffect);
    }


    private void CheckMaxTongueDistance()
    {
        float currentDistance = Vector3.Distance(tongueOrigin.position, tongueEnd.position);

        if (currentDistance >= maxTongueDistance) {
            shootTongue = false;
        }
    }

    //PLAYER COLOR
    private void ChangePlayerColor(ColorType colorType) {
        for (int attempts = 0; attempts < colorTypes.Length; attempts++) {
            if (!ColorManager.Instace.GetAssigneds(colorTypes[colorIndex])) {
                onPaintPlayer?.Invoke(colorTypes[colorIndex]);
                onChangeColor?.Invoke(colorIndex);
                return;
            }
            if(lastColorChangeRight)
                SwapRightColor();
            else
                SwapLeftColor();
        }
        onPaintPlayer?.Invoke(ColorType.Default);
        onChangeColor?.Invoke(colorIndex);
    }

    private void SwapRightColor() {
        colorIndex = (colorIndex + 1) % colorTypes.Length;
        lastColorChangeRight = true;
    }

    private void SwapLeftColor()
    {
        colorIndex = (colorIndex - 1 + colorTypes.Length) % colorTypes.Length;
        lastColorChangeRight = false;
    }

    
    //SETTERS & GETTERS
    private void setShootTongue() {
        if (canShootAgain && !inWater) {
            PlayerAnimations.Instance.ChangeHeadAnimation(HeadAnim.Open);
            shootTongue = true;
            canShootAgain = false;
            canCheckCollisions = true;
            onShootingTongue?.Invoke();
            
        }
    }

    public ColorType[] GetColorTypes()
    {
        return colorTypes;
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
        PlayerInputs.Instance.onSwapRightColor += SwapRightColor;
        PlayerInputs.Instance.onSwapLeftColor += SwapLeftColor;
        WaterEffect.onWater += InWater;
    }

    private void OnDisable()
    {
        PlayerInputs.Instance.onShoot -= setShootTongue;
        PlayerInputs.Instance.onSwapRightColor -= SwapRightColor;
        PlayerInputs.Instance.onSwapLeftColor -= SwapLeftColor;
        WaterEffect.onWater -= InWater;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.right * 3);

        Gizmos.DrawWireSphere(tongueEnd.position, detectionRadius);
    }
}
