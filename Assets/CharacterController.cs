using UnityEngine;

public class CharacterController : MonoBehaviour
{
    // Variables to control player movement
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public float mass = 2f;
    public Transform groundCheck;   // Reference to check if the player is on the ground
    public LayerMask groundLayer;   // To specify what layers count as ground
    public float JUMP_DELAY = 0.5f;

    private bool isGrounded;
    private float moveInputX;
    private Vector3 velocity;
    private Vector3 moveDirection;
    private bool isJumping;
    private float gravity = -9.81f; // Simulate gravity
    private float verticalVelocity = 0f;
    private float jumpDelayTimer = 0f;
    private Animator animator;
    private Transform originalParent = null;
    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        // Check if the player is on the ground
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, groundLayer);

        if (isGrounded)
        {
            // Get input for horizontal and vertical movement
            moveInputX = -Input.GetAxis("Horizontal"); // A/D or Left/Right arrow keys
            animator.SetBool("walk", moveInputX != 0);
        }

        transform.GetChild(0).localScale = new Vector3(moveInputX > 0 ? 1 : -1, 1, 1);

        if (isGrounded)
        {
            // Get the platform's transform when first detected
            Collider[] hitColliders = Physics.OverlapSphere(groundCheck.position, 0.2f, groundLayer);

            if (hitColliders != null && hitColliders[0].CompareTag("Platform"))
            {
                // Save the original parent and set the player's parent to the platform
                transform.SetParent(hitColliders[0].transform);
            }
            else
            {
                transform.SetParent(originalParent);
            }
        }
        
        // Reset vertical velocity when grounded
        if (isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = 0f;
            isJumping = false;
        }

        // Jump logic
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            isJumping = true;
            jumpDelayTimer = 0f;
            animator.SetTrigger("jump");
        }

        if (isJumping && jumpDelayTimer <= JUMP_DELAY)
        {
            jumpDelayTimer += Time.deltaTime;
            if(jumpDelayTimer > JUMP_DELAY)
            {
                verticalVelocity = jumpForce;
            }
        }

        // Apply gravity when not grounded
        if (!isGrounded)
        {
            verticalVelocity += mass * gravity * Time.deltaTime;
        }

        // Apply the movement and jumping logic
        moveDirection = transform.right * moveInputX;
    }

    void FixedUpdate()
    {
        // Apply horizontal movement and vertical velocity (for jump and gravity)
        velocity = new Vector3(moveDirection.x * moveSpeed, verticalVelocity, moveDirection.z * moveSpeed);

        // Since it's kinematic, move the object manually using its position
        transform.position += velocity * Time.fixedDeltaTime;
    }

    public void CastSuperPower()
    {
        animator.SetTrigger("superPower");
    }
}