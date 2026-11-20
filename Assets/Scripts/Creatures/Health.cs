using System;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(SpriteRenderer))]
public class Health : MonoBehaviour
{
    [SerializeField] public int hp = 1;
    [SerializeField] public float damageDuration = 0.4f;
    [SerializeField] private UnityEvent onDeath, onHit;
    
    private SpriteRenderer spriteRenderer;
    private bool isDamaged = false;
    private float damagedTimer = 0.0f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (isDamaged)
        {
            damagedTimer += Time.deltaTime;
            if (damagedTimer >= damageDuration)
            {
                isDamaged = false;
                damagedTimer = 0.0f;
            } 
            else if (damagedTimer >= 0.01)
            {
                spriteRenderer.color = Color.white;
            }
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Damage(int damage)
    {
        if (isDamaged) return;
        spriteRenderer.color = Color.black;
        isDamaged = true;
        hp -= damage;
        onHit?.Invoke();
        
        if (hp <= 0)
        {
            HandleDeath();
        }
    }

    void HandleDeath()
    {
        onDeath?.Invoke();
    }
}
