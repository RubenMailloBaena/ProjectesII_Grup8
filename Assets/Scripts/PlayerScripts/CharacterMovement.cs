using UnityEngine;


public class CharacterMovement: MonoBehaviour
{
    [Header("Moviment")]
    //PLAYER MOVEMENT
    [SerializeField] private float playerSpeed = 5f;
    private bool facingRight = true;
    private bool canFlip = true;
    private Vector2 movementDirection;

    [Header("Jump")]
    //PLAYER JUMP
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float acelerationRate = 0.2f;
    [SerializeField] private float startGravity = 10;
    private bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGorunded;
    private Vector3 lastJumpPosition;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        movementDirection.x = Input.GetAxisRaw("Horizontal");

        ApplyMovement();
        FlipPlayer();
        CheckJumpingLogic();
    }

    private void ApplyMovement() {
        rb.AddForce(new Vector2(movementDirection.x * playerSpeed * Time.deltaTime, 0));    
    }

    private void FlipPlayer(){
        
        if (canFlip) {
            if ((movementDirection.x > 0 && !facingRight) || (movementDirection.x < 0 && facingRight))
            {
                facingRight = !facingRight;

                Vector3 currentRotation = transform.eulerAngles;
                currentRotation.y += 180f;
                transform.eulerAngles = currentRotation;
            }
        }
    }

    private void CheckJumpingLogic() { 
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGorunded);

        if (!isGrounded) { 
            rb.drag -= acelerationRate;
        }
        else { 
            rb.drag = startGravity;
        }
    }

    private void PlayerJump() {

        if (isGrounded) {
            rb.AddForce(Vector2.up * jumpForce);
            lastJumpPosition = transform.position;
        }
    }

    private void CanNotFlip() {
        canFlip = false;
    }

    private void CanFlip() {
        canFlip = true;
    }


    private void OnEnable()
    {
        TongueController.onShootingTongue += CanNotFlip;
        TongueController.onNotMovingTongue += CanFlip;
        PlayerInputs.onJump += PlayerJump;
    }

    private void OnDisable()
    {
        TongueController.onShootingTongue -= CanNotFlip;
        TongueController.onNotMovingTongue -= CanFlip;
        PlayerInputs.onJump -= PlayerJump;
    }

    public Vector3 getLastJumpPosition() { 
        return lastJumpPosition;
    }
}
