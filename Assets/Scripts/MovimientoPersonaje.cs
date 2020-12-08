using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class MovimientoPersonaje : MonoBehaviour
{
    public float speed;
    public float jumpForce;

    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius;

    private Rigidbody2D rigidbody;
    private Animator animator;
    private Vector2 movement;

    private bool isGrounded;
    private bool isAttacking;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (!isAttacking)
        {
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            movement = new Vector2(horizontalInput, 0f);
            if (horizontalInput < 0f)
            {
                transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
            }
            else if (horizontalInput > 0f)
            {
                transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
            }
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        if(Input.GetButtonDown("Fire1") && isGrounded && !isAttacking)
        {
            movement = Vector2.zero;
            rigidbody.velocity = Vector2.zero;
            animator.SetTrigger("Attacking");
        }
    }

    void FixedUpdate()
    {
        if (!isAttacking)
        {
            float horizontalVelocity = movement.normalized.x * speed;
            rigidbody.velocity = new Vector2(horizontalVelocity, rigidbody.velocity.y);
        }
    }

    void LateUpdate()
    {
        
        animator.SetBool("Idle", movement == Vector2.zero);
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetFloat("VerticalVelocity", rigidbody.velocity.y);
        isAttacking = animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack");
    }
}
