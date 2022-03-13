using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed;
    public float jumpForce;
    [SerializeField] private int extraJumpValue;
    private int extraJumps; 
    
    private float moveInput;
    private Rigidbody2D rb;

    public bool facingRight = true;
    private bool isGrounded;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float checkRadius;
    [SerializeField] private LayerMask whatIsGround;

    public static bool invertControls = false;

    private void Start()
    {
        extraJumps = extraJumpValue;
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        moveInput = Input.GetAxisRaw("Horizontal");

        if (!invertControls)
        {
            rb.velocity = new Vector2(moveInput * movementSpeed, rb.velocity.y);
            
            if (!facingRight && moveInput > 0)
            {
                Flip();
            }
            else if (facingRight && moveInput < 0)
            {
                Flip();
            }
        }
        else
        {
            rb.velocity = new Vector2(-moveInput * movementSpeed, rb.velocity.y);

            if (!facingRight && moveInput < 0)
            {
                Flip();
            }
            else if (facingRight && moveInput > 0)
            {
                Flip();
            }
        }
    }

    private void Update()
    {
        if (isGrounded)
            extraJumps = extraJumpValue;

        if(Input.GetKeyDown(KeyCode.Space) && extraJumps > 0)
        {
            rb.velocity = Vector2.up * jumpForce;
            extraJumps--;
        } else if (Input.GetKeyDown(KeyCode.Space) && extraJumps == 0 && isGrounded)
        {
            rb.velocity = Vector2.up * jumpForce;
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    public bool InvertControls()
    {
        return invertControls = !invertControls;
    }
}
