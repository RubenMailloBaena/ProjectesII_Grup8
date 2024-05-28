using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;


public class TongueController : MonoBehaviour
{
    //Singletone pattern
    public static TongueController instance;

    [Header("BLOQUEAR EFECTOS ESCENAS")]
    [SerializeField] private bool canUseStrech = true;
    [SerializeField] private bool canUseElastic = true;
    [SerializeField] private bool canUseWater = true;
    private ColorType[] colorTypes;
    private int colorIndex = 0;

    [Header("COLOR ROULETTE")] 
    [SerializeField] private float arrowSpeed;
    private GameObject rouletteInstance;
    private Image[] rouletteColors = new Image[3];
    private GameObject arrow;
    private int arrowAngle;
    private int colorsToPaint = 0;

    [Header("OTHER GAMEOBJECTS")]
    [SerializeField] private Transform tongueEnd;
    [SerializeField] private Transform tongueOrigin;
    [SerializeField] private GameObject roulettePrefab;
    [SerializeField] private GameObject gameManager;

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
    
    private LineRenderer lineRenderer;
    

    private void Awake()
    {
        if (instance == null)
            instance = this;
        
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;

        SetColorsCanShoot();
        InitializeRouletteColors();
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
        ArrowLogic();
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
               if (hitColliders[i].gameObject.tag.Equals("Door")) {
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
        IColorEffect currentEffect = ColorManager.instance.GetColorEffect(colorTypes[colorIndex]);
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
            if (!ColorManager.instance.GetAssigneds(colorTypes[colorIndex])) {
                onPaintPlayer?.Invoke(colorTypes[colorIndex]);
                RepaintRoulette();
                return;
            }
            if(lastColorChangeRight)
                SwapRightColor();
            else
                SwapLeftColor();
        }
        onPaintPlayer?.Invoke(ColorType.Default);
        RepaintRoulette();
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
    
    
    //RULETA DE COLORES
    private void InitializeRouletteColors()
    {
        rouletteInstance = Instantiate(roulettePrefab, transform.position, Quaternion.identity);
        GetAllRouletteReferences();
        colorsToPaint = colorTypes.Length;
        for (int i = 0; i < rouletteColors.Length; i++)
        {
            if (i < colorTypes.Length)
                rouletteColors[i].color = ColorManager.instance.GetColor(colorTypes[i]);
            else
                rouletteColors[i].color = ColorManager.instance.GetColor(ColorType.Default);
        }
    }
    
    private void GetAllRouletteReferences()
    {
        for (int i = 1; i <= 3; i++)
            rouletteColors[i - 1] = rouletteInstance.transform.GetChild(0).transform.Find("Color" + i).GetComponent<Image>();
        arrow = rouletteInstance.transform.GetChild(0).transform.Find("ArrowPivot").gameObject;
    }
    
    public void RepaintRoulette()
    {
        for (int i = 0; i < colorsToPaint; i++)
            rouletteColors[i].color = ColorManager.instance.GetColor(colorTypes[i]);
    
        if (colorIndex == 0)
            arrowAngle = 0;
        else if (colorIndex == 1)
            arrowAngle = 120;
        else
            arrowAngle = 240;
        
        CheckIfAllColorsDefault();
    }
    
    private void CheckIfAllColorsDefault()
    {
        bool allGrey = true;
        for (int i = 0; i < rouletteColors.Length; i++)
        {
            if (rouletteColors[i].color != ColorManager.instance.GetColor(ColorType.Default))
            {
                allGrey = false;
                break;
            }
        }
        arrow.SetActive(true);
        if (allGrey)
            arrow.SetActive(false);
    }
    
    private void ArrowLogic()
    {
        Quaternion currentRotation = arrow.transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0, 0, arrowAngle);
        Quaternion newRotation = Quaternion.Lerp(currentRotation, targetRotation, arrowSpeed * Time.deltaTime);
        arrow.transform.rotation = newRotation;
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
