using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CircleCollider2D), typeof(SpriteRenderer))]

public class MagicAttack : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private SpriteRenderer spriteFire;
    [SerializeField] private AudioSource windUpAudio;
    [SerializeField] private AudioSource attackAudio;
    
    [Header("Attack")]
    [SerializeField] private float windupTime = 0.4f;
    [SerializeField] private float fireTime = 0.5f;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private int damage = 10;
    
    [Header("Events")]
    [SerializeField] private UnityEvent onEuthanized;

    // Components
    private ContactFilter2D filter;
    private CircleCollider2D circleCollider;
    private SpriteRenderer spriteBase;
    

    void Awake()
    {
        // Setup filter for collider
        filter = new ContactFilter2D();
        filter.SetLayerMask(enemyMask);
        filter.useTriggers = false;
        
        // Collider
        circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.isTrigger = true;
        
        // Sprites
        spriteBase = GetComponent<SpriteRenderer>();
        spriteBase.enabled = false;
        spriteFire.enabled = false;
    }
    

    public void HintPosition(Vector2 position)
    {
        transform.position = position;
        spriteBase.color = SetAlpha(spriteBase.color, 0.2f);
        spriteBase.enabled = true;
    }
    
    
    public Coroutine Attack(Vector2 position)
    {
        return StartCoroutine(AttackRoutine(position));
    }

    IEnumerator AttackRoutine(Vector2 position)
    {
        // Windup, circle fully visible
        transform.position = position;
        spriteBase.color = SetAlpha(spriteBase.color, 1f);
        windUpAudio.Play();
        yield return StartCoroutine(Pause(windupTime));
        
        // Attack, fire spawns
        spriteFire.enabled = true;
        attackAudio.Play();
        List<Collider2D> hits = new List<Collider2D>();
        circleCollider.Overlap(filter, hits);
        for (int i = 0; i < hits.Count; i++)
        {
            if (hits[i].TryGetComponent(out Health health))
            {
                health.Damage(damage);
                if (health.hp <= 0)
                {
                    onEuthanized?.Invoke();
                }
            }
        }
        yield return StartCoroutine(Pause(fireTime));
        
        spriteFire.enabled = false;
        spriteBase.enabled = false;
    }
    
    
    // Sets alpha to make sprite see-through
    Color SetAlpha(Color color, float alpha)
    {
        color.a = alpha;
        return color;
    }

    IEnumerator Pause(float seconds)
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(seconds);
        Time.timeScale = 1;
    }
    
}
