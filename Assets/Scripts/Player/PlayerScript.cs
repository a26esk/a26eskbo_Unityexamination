using System;
using System.Collections;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;


[RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D), typeof(Animator))] 
[RequireComponent(typeof(SpriteRenderer), typeof(AudioListener), typeof(PlayerInput))]
public class PlayerScript : MonoBehaviour
{
    public float maxSpeed = 7;
    public bool controlEnabled { get; set; } = true;

    
    [Tooltip("Distance to spawn attack from player")]
    [SerializeField] private MagicAttack magicAttack;
    [SerializeField] private float attackOffset;
    
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;
    private PlayerInput playerInput;

    private Vector2 moveInput = new Vector2(0, 0); // moveInput

    [Tooltip("Shows the translucent attack circle")]
    private bool showAttackHint = false;
    
    [Tooltip("Saves where the player is looking at, as inputMove defaults back to zero")]
    private Vector2 lookingAt = Vector2.zero;
    [Tooltip("looking at + an offset + own position. Is sent to magickAttack")]
    private Vector2 attackPos = Vector2.zero;
    
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        playerInput = GetComponent<PlayerInput>();
        
        // Set gravity scale to 0 so player won't "fall" 
        rb.gravityScale = 0;
    }
    
    private void FixedUpdate()
    {
        HandleMovement();
        HandleAnimation();
        
        // Handle attack
        if (showAttackHint)
        {
            magicAttack.HintPosition(GetAttackPos());
        }
    }
    
    // Handle Move-input
    // This method can be triggered through the UnityEvent in PlayerInput
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>().normalized;
        
        // Limits to only one moveInput. Either x or y.
        if (Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y)) moveInput.y = 0;
        else moveInput.x = 0;
        moveInput = moveInput.normalized;

        // KEEP moveInput if nothing is pressed
        if (moveInput == Vector2.zero) return;
        lookingAt = moveInput;
        anim.SetFloat("DirX", moveInput.x);
        anim.SetFloat("DirY", moveInput.y);
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        float attackInput = context.ReadValue<float>();
        if (attackInput.Equals(1))
        {
            showAttackHint = true;
        }
        else
        {
            showAttackHint = false;
            StartCoroutine(Attack());
        }
    }


    // Set velocity based on moveInput of input and maxSpeed
    void HandleMovement()
    {
        if (controlEnabled)
        {
            rb.linearVelocity = moveInput.normalized * maxSpeed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    void HandleAnimation()
    {
        anim.SetFloat("Speed", rb.linearVelocity.magnitude);
        if (rb.linearVelocity.x < -0.01)
        {
            sr.flipX = true;
        } else if (rb.linearVelocity.x > 0.01)
        {
            sr.flipX = false;
        }
    }
    
    IEnumerator Attack()
    {
        playerInput.DeactivateInput();
        yield return magicAttack.Attack(GetAttackPos());
        playerInput.ActivateInput();
    }


    Vector2 GetAttackPos()
    {
        Vector2 newOffset = Vector2.zero;
        if (lookingAt == Vector2.left || lookingAt == Vector2.right)
        {
            newOffset = new Vector2(transform.position.x, transform.position.y - 0.15f) + (attackOffset * lookingAt);
        }
        else if (lookingAt == Vector2.up)
        {
            newOffset = new Vector2(transform.position.x, transform.position.y - attackOffset/2) + (attackOffset * lookingAt);
        }
        else
        { 
            newOffset = new Vector2(transform.position.x, transform.position.y) + (attackOffset * Vector2.down);
        }
        return newOffset;
    }
}
