using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Health Settings")]
    public float health;
    public float maxHealth;
    public float baseMaxHealth;
    [Header("Movement speed settings")]
    public float moveSpeed;
    public float baseMoveSpeed;
    [HideInInspector]
    public float healAmount;
    [HideInInspector]
    public float damageAmount;
    [Header("Regen-rate settings")]
    public float regenRate = 0f;
    protected float regenTimer = 0f;
    [Header("Misc")]
    public Player player;// Reference to Player class
    protected Animator animator;
    public bool IsFreeze;
    public bool IsDead;
    public SpriteRenderer spriteRenderer;


    // Start is called before the first frame update
    void Start()
    {
        Initialise();
    }

    protected virtual void Update()
    {
        if (!IsDead)
        {
            if (health <= 0)
            {
                Die();
                Debug.Log(name + " health at 0");
            }
        }        
    }


    public void Heal(float healAmount)
    {
        health = health + healAmount;
        health = Mathf.Clamp(health, 0f, maxHealth); // Ensure health doesn't go above maxHealth
    }

    public void takeDamage(float damageAmount)
    {
        if (!IsDead)
        {
            health -= damageAmount;
            health = Mathf.Clamp(health, 0f, maxHealth); // Ensure health doesn't go below 0
            StartCoroutine(DamageVisual());
            Debug.Log(name + " took damage: " + damageAmount + ", Remaining health: " + health);  // Debugging line

            if (health <= 0f)
            {
                Die();
            }
        }
        
    }

    public virtual void Die()
    {
        Destroy(gameObject); // Destroy the enemy
        IsDead = true;
        Debug.Log(name + "Dead");
        player.IncreaseKills();
    }

    protected IEnumerator DamageVisual()
    {
        //Adds light transparent red to the sprite to show damage
        spriteRenderer.color = new Color(0.8f, 0.1f, 0.1f, 1);

        yield return new WaitForSeconds(0.25f);

        spriteRenderer.color = Color.white;
    }

    protected virtual void Initialise()
    {
        player = FindObjectOfType<Player>();
        health = maxHealth; //Sets the health as max health
        animator = GetComponent<Animator>();
    }
}
