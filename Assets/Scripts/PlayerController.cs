using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;
    private float speedMultiplier;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public bool readyToJump;

    public int maxHp;
    public int hp;
    public TMP_Text healthText;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    [Header("Spawning")]
    [SerializeField] private Transform[] spawnPoints;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;
    bool sprinting;

    Vector3 moveDirection;

    Rigidbody rb;

    private void Start()
    {
        speedMultiplier = 1f;

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        // Spawn player at the correct spawn point, nothing should happen in test scene
        if (spawnPoints.Length > 0 && GameManager.Instance != null)
        {
            int currentStage = GameManager.Instance.getCurrentStage();
            if (currentStage == 0) currentStage = 1;                    // Work around for when just loading into Level1 not through the menu

            rb.position = spawnPoints[currentStage - 1].position;
        }

    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position + new Vector3(0, 0.2f, 0),Vector3.down, playerHeight * 0.5f + 0.1f, whatIsGround);
        Debug.DrawRay(transform.position + new Vector3(0, 0.2f, 0), Vector3.down * (playerHeight * 0.5f + 0.1f), Color.red);
        MyInput();
        SpeedControl();
        if(grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        sprinting = Input.GetKey(sprintKey);

        if(Input.GetKeyDown(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        
        float baseForce = moveSpeed * 10f;
        float sprintMultiplier = sprinting ? 1.5f : 1f;
        float groundMultiplier = grounded ? 1f : airMultiplier;

        float finalForce = baseForce * sprintMultiplier * groundMultiplier * speedMultiplier;

        rb.AddForce(moveDirection.normalized * finalForce, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //limit velocity if needed
        if(flatVel.magnitude > moveSpeed){
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        healthText.text = "HP: " + hp;
        if (hp <= 0)
        {
            Debug.Log("Player died!");
        }
    }

    public void setMaxHealth( int health )
    {
        maxHp = health;
        healToMax();
    }

    public void Heal(int amount)
    {
        hp += amount;
        healthText.text = "HP: " + hp;
    }

    public void healToMax()
    {
        hp = maxHp;
        healthText.text = "HP: " + hp;
    }

    public void setSpeedMultiplier( float multiple )
    {
        speedMultiplier = multiple;
    }
}