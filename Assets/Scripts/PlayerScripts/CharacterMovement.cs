using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterMovement : MonoBehaviour
{
    private static CharacterMovement instance;
    public static CharacterMovement Instance
    {
        get { 
            if(instance == null)
                instance = FindAnyObjectByType<CharacterMovement>();
            return instance;
        }
    }

    [Header("MOVEMENT")]
    //PLAYER MOVEMENT
    [SerializeField] private float playerSpeed = 5f;
    [SerializeField] private float initialGravityScale;
    private bool canFlip = true;
    private Vector2 movementDirection;

    [Header("JUMP")]
    //PLAYER JUMP
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float fallMultiplier;
    public Transform groundCheck;
    public float checkRadius;
    private bool isGrounded;
    public LayerMask whatIsGorunded;
    private Vector3 lastJumpPosition;
    private Vector2 vecGravity;

    [Header("UNDER WATER")] 
    [SerializeField] private float speedInWater = 10f;
    [SerializeField] private float jumpForceInWater = 10f;
    [SerializeField] private float linearDrag = 10f;
    [SerializeField] private float gravityInWater = 0.5f;
    private bool inWater = false;

    private Rigidbody2D rb;
    private bool facingRight = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        vecGravity = new Vector2(0, -Physics2D.gravity.y);
    }

    private void Update()
    {
        movementDirection.x = Input.GetAxisRaw("Horizontal");
        ManagePlayerAnimations();
    }

    private void FixedUpdate()
    {
        if(inWater)
            ApplyMovementInWater();
        else
            ApplyMovement();

        FlipPlayerByMovement();
        CheckJumpingLogic();
    }

    private void ApplyMovementInWater()
    {
        rb.drag = linearDrag;
        rb.gravityScale = gravityInWater;
        rb.AddForce(new Vector2(movementDirection.x * speedInWater , 0));
    }

    private void ApplyMovement() {
        rb.gravityScale = initialGravityScale;
        rb.drag = 0;
        rb.velocity = new Vector2(movementDirection.x * playerSpeed , rb.velocity.y);
    }

    private void FlipPlayerByMovement(){
        
        if (canFlip) {
            if ((movementDirection.x > 0 && !facingRight) || (movementDirection.x < 0 && facingRight))
                RotatePlayer();
        }
    }

    private void RotatePlayer() {
        facingRight = !facingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void CheckJumpingLogic() {
        Collider2D collider = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGorunded);

        if (collider == null)
        {
            isGrounded = false;
        }
        else {
            if (collider.gameObject.layer == 6) //gorund layer
            {
                isGrounded = true;
                lastJumpPosition = transform.position;
                transform.parent = null;
            }
            else if (collider.gameObject.layer == 7) //obstacles
            { 
                isGrounded = true;
                if (collider.gameObject.GetComponent<ObstacleEffectLogic>().getCurrentColorType() != ColorType.Elastic)
                    lastJumpPosition = transform.position;
                if (collider.gameObject.GetComponent<ObstacleEffectLogic>().LastColorTypeWasStrech())
                {
                    if (transform.parent != collider.transform.parent)
                    {
                        transform.parent = collider.transform.parent;
                    }
                }
                else
                    transform.parent = null;
            }
        }

        if (inWater) {
            rb.AddForce(-vecGravity * (fallMultiplier * Time.deltaTime));
        }
        else
        {
            if (rb.velocity.y < 0)
            {
                rb.velocity -= vecGravity * (fallMultiplier * Time.deltaTime);
            }
        }
    }

    private void PlayerJump() {
        transform.parent = null;
        if (isGrounded && !inWater)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            Debug.Log("jumping NORMAL " + jumpForce );
        } 
        else if (inWater)
        {
            // rb.AddForce(Vector2.up * jumpForceInWater);
            // Debug.Log("SWIMMING " + jumpForceInWater);
            rb.velocity = new Vector2(rb.velocity.x, jumpForceInWater*Time.deltaTime);
            Debug.Log("SWIMMING " + jumpForceInWater);
        }
    }


    private void ManagePlayerAnimations() {
        if (rb.velocity.y > 3 && !isGrounded&& !inWater)
        { //jump
            PlayerAnimations.Instance.ChangeAnimation(PlayerAnim.Jump);
        }
        else if (rb.velocity.y > 0 &&  inWater) PlayerAnimations.Instance.ChangeAnimation(PlayerAnim.Swim);
        else if (rb.velocity.y < 0 && inWater) PlayerAnimations.Instance.ChangeAnimation(PlayerAnim.SwimDown);
        else if (rb.velocity.y < 3 && !isGrounded) { 
            PlayerAnimations.Instance.ChangeAnimation(PlayerAnim.Fall);
        } //fall
        else if (movementDirection.x != 0 && isGrounded) //walk
            PlayerAnimations.Instance.ChangeAnimation(PlayerAnim.Walk);
        else PlayerAnimations.Instance.ChangeAnimation(PlayerAnim.Idle);
    }



    private void CanNotFlip() {
        canFlip = false;
    }

    private void CanFlip() {
        canFlip = true;
    }

    private void InWater() {
        inWater = !inWater;
    }

    public bool GetFacingRight() {
        return facingRight;
    }

    public Vector3 getLastJumpPosition()
    {
        return lastJumpPosition;
    }

    public void SetPlayerPosition(Vector3 checkPointPosition)
    {
        transform.position = checkPointPosition;
    }


    private void OnEnable()
    {
        TongueController.Instance.onShootingTongue += CanNotFlip;
        TongueController.Instance.onNotMovingTongue += CanFlip;
        PlayerInputs.Instance.onJump += PlayerJump;
        WaterEffect.onWater += InWater;
    }

    private void OnDisable()
    {
        TongueController.Instance.onShootingTongue -= CanNotFlip;
        TongueController.Instance.onNotMovingTongue -= CanFlip;
        PlayerInputs.Instance.onJump -= PlayerJump;
        WaterEffect.onWater -= InWater;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(groundCheck.transform.position, checkRadius);
    }

}

