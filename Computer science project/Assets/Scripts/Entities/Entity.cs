using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Health Settings")]
    public float health;
    public float maxHealth;
    public float baseMaxhealth;
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
    public bool IsFreeze;


    // Start is called before the first frame update
    void Start()
    {
        Initialise();
    }

    protected virtual void Update()
    {
        if (health <= 0)
        {
            Die();
            Debug.Log(name + " health at 0");
        }
    }


    public void Heal(float healAmount)
    {
        health = health + healAmount;
        health = Mathf.Clamp(health, 0f, maxHealth); // Ensure health doesn't go above maxHealth
    }

    public void takeDamage(float damageAmount)
    {
        health -= damageAmount;
        health = Mathf.Clamp(health, 0f, maxHealth); // Ensure health doesn't go below 0
        Debug.Log(name + " took damage: " + damageAmount + ", Remaining health: " + health);  // Debugging line

            if (health <= 0f)
            {
                Die();
            }
    }

    public virtual void Die()
    {
        Destroy(gameObject); // Destroy the enemy
        Debug.Log(name + "Dead");
        player.IncreaseKills();
    }

    protected virtual void Initialise()
    {
        player = FindObjectOfType<Player>();
        health = maxHealth; //Sets the health as max health
    }
}
