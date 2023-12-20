using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComponent : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;
    private float dirX = 0f;
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private Vector2 playerVelocity;
    [SerializeField] private float jumpForce = 14f;
    [SerializeField] private float baseSwingForce = 30f;
    private float currentSwingForce;

    private enum MovementState
    {
        idle, running, jumping, falling
    }
    [SerializeField] private AudioSource jumpSoundEffect;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

     private void Update()
    {
        Movement movementComponent = GetComponent<Movement>();
        if (Input.GetKeyDown(KeyCode.E))
        {
            currentSwingForce = baseSwingForce * 2f;
        }
        else
        {
            currentSwingForce = baseSwingForce;
        }

        if (movementComponent != null && movementComponent._Distancejoint != null && movementComponent._Distancejoint.enabled && !isGrounded())
        {
            /*Vector2 swingForce = new Vector2(30, 0);
           rb.AddForce(swingForce, ForceMode2D.Force);*/
            if (Mathf.Approximately(rb.velocity.x, 0f))
            {
                Vector2 swingForce = new Vector2(currentSwingForce, 0);
                rb.AddForce(swingForce, ForceMode2D.Force);
            }
        }
        else
        {
             
            dirX = Input.GetAxisRaw("Horizontal");
            rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
            playerVelocity = rb.velocity;
            if (Input.GetButtonDown("Jump") && isGrounded())
            {
                jumpSoundEffect.Play();
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }

            UpdateAnimationState();
        }
    } 
    

    private void UpdateAnimationState()
    {
        MovementState state;

        if (dirX > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (dirX < 0f)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }

        anim.SetInteger("state", (int)state);
    }
    private bool isGrounded()
    {
        RaycastHit2D groundCheck = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, 0.1f, jumpableGround);
        return groundCheck.collider != null;
    }
}