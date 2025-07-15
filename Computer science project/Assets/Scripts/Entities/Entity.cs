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
    [Header("Regen-rate settings")]
    public float regenRate = 0f;
    protected float regenTimer = 0f;
    protected float regenAccumulator = 0f;
    [Header("Armour Settings")]
    public float armour = 0f; // Armour value to reduce damage taken

    [Header("Misc")]
    public Player player;// Reference to Player class
    protected Animator animator;
    public bool IsFreeze;
    public bool IsDead;
    public SpriteRenderer spriteRenderer;
    private Color baseColour;


    // Start is called before the first frame update
    void Start()
    {
        Initialise();
    }

    protected virtual void Update()
    {
        if (regenRate > 0f && health < maxHealth)
        {
            regenAccumulator += regenRate * Time.deltaTime; // regenRate = HP per second
            if (regenAccumulator >= 1f)
            {
                int healAmount = Mathf.FloorToInt(regenAccumulator);
                Heal(healAmount);
                regenAccumulator -= healAmount; // remove healed amount
            }
        }
    }

    protected void FixedUpdate()
    {
        if (regenRate > 0f && !IsDead)
        {
            regenTimer += Time.fixedDeltaTime;
            if (regenTimer >= 1f / regenRate)
            {
                Heal(1f); // Heal 1 health point every second
                regenTimer = 0f; // Reset the timer
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
        float damageAfterArmour = damageAmount * (1 - armour / 100f);
        if (IsDead) { return; } // Prevent damage if already dead
        health -= damageAfterArmour;
        health = Mathf.Clamp(health, 0f, maxHealth); // Ensure health doesn't go below 0
        StartCoroutine(DamageVisual());
        Debug.Log(name + " took damage: " + damageAmount + ", Remaining health: " + health);  // Debugging line

        if (health <= 0f)
        {
            Die();
        }        
    }

    public virtual void Die()
    {
        IsDead = true;
        Destroy(gameObject); // Destroy the enemy
        Debug.Log(name + "Dead");
        player.IncreaseKills();
    }

    protected IEnumerator DamageVisual()
    {
        spriteRenderer.color = baseColour; // Reset to base color
        //Adds light transparent red to the sprite to show damage
        spriteRenderer.color = new Color(0.8f, 0.1f, 0.1f, 1);

        yield return new WaitForSeconds(0.25f);

        spriteRenderer.color = baseColour;
    }

    protected virtual void Initialise()
    {
        player = FindObjectOfType<Player>();
        health = maxHealth; //Sets the health as max health
        animator = GetComponent<Animator>();
        baseColour = spriteRenderer.color; // Store the base color of the sprite
    }
}
