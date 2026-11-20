using System;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Health), typeof(SpriteRenderer), typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider2D), typeof(Rigidbody2D), typeof(AudioSource))]
[RequireComponent(typeof(CircleCollider2D))]
public class FoulCreature : MonoBehaviour
{
    [Header("Sprint")] 
    [SerializeField] private float sprintForce = 12f;
    [SerializeField] private float sprintRecoveryTime = 3.0f;
    [SerializeField] private float sprintTimeRandom = 2.0f;
    [SerializeField] private LayerMask detectionLayer;
    [Header("Death")]
    [SerializeField] private UnityEvent onDespawn;
    [SerializeField] private float despawnAfterTime = 2.0f;
    [SerializeField] private ParticleSystem deathParticles;
    
    
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private CircleCollider2D triggerCollider;
    
    private bool dead = false;
    private bool canSprint = true;
    private float sprintTimer = 0;
    private float realSprintRecoveryTime = 0.0f;
    private float despawnTimer = 0.0f;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        
        // Trigger collider
        triggerCollider = GetComponent<CircleCollider2D>();
        triggerCollider.isTrigger = true;
        triggerCollider.callbackLayers = detectionLayer;
        
        rb.gravityScale = 0;
    }
    
    private void FixedUpdate()
    {
        if (!canSprint)
        {
            sprintTimer += Time.deltaTime;
            if (sprintTimer >= realSprintRecoveryTime)
            {
                canSprint = true;
                sprintTimer = 0;
            }
        }
        
        if (dead)
        {
            despawnTimer += Time.fixedDeltaTime;
            if (despawnTimer > despawnAfterTime)
            {
                OnDespawn();
            }
        }
        
        // Handle animations
        if (Mathf.Abs(rb.linearVelocity.magnitude) > 0.01f)
        {
            if (rb.linearVelocityX < -0.01f) spriteRenderer.flipX = false;
            else if (rb.linearVelocityX > 0.01f) spriteRenderer.flipX = true;
            
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }
    
    public void OnDied()
    {
        animator.SetBool("isDead", true);
        audioSource.Stop();
        rb.simulated = false;
        dead = true;
    }

    public void OnDespawn()
    {
        if (deathParticles) Instantiate(deathParticles, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
        onDespawn.Invoke();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (canSprint && other.gameObject != gameObject)
        {
            var direction = (rb.transform.position - other.transform.position).normalized;
            rb.AddForce(direction * sprintForce, ForceMode2D.Impulse);
            realSprintRecoveryTime = sprintRecoveryTime + Random.Range(-sprintTimeRandom, sprintTimeRandom);
            canSprint = false;
        }
    }
    
    void AddRandomVelocity()
    {
        var direction = Random.insideUnitCircle.normalized;
        rb.AddForce(direction*sprintForce);
        Debug.unityLogger.Log("Adding random velocity");
    }
        
}
