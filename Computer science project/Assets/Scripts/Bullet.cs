using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bullet : MonoBehaviour
{
    public enum ShooterType { Player, Enemy }
    public ShooterType shooter = ShooterType.Player;

    public float speed;
    public float bulletLifetime;
    public Vector2 bulletDirection;
    private Gun gun;  // Reference to the Gun
    protected bool hasHit = false;
    public float damage;

    protected virtual void Start()
    {
        // Destroy the bullet after a set time (bulletLifetime)
        Destroy(gameObject, bulletLifetime);
    }

    void Update()
    {
        transform.Translate(bulletDirection * speed * Time.deltaTime);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        // Handle Player collision
        if (shooter == ShooterType.Player && collider.CompareTag("Enemy") && !hasHit)
        {
            var enemy = collider.GetComponent<Enemy>();
            if (enemy != null) // Check if the enemy component exists
            {
                HitEnemy(enemy);
            }
        }

        // Handle Enemy collision
        if (shooter == ShooterType.Enemy && collider.CompareTag("Player") && !hasHit)
        {
            var player = collider.GetComponent<Player>();
            if (player != null) // Check if the player component exists
            {
                HitPlayer(player);
            }
        }

        // Handle Wall collision
        if (collider.CompareTag("Wall") || collider.GetComponent<TilemapCollider2D>() != null)
        {
            HitWall(collider);
        }
    }

    protected virtual void HitEnemy(Enemy enemy)
    {
        hasHit = true;
        enemy.takeDamage(damage); // Directly use the Enemy reference
        Destroy(gameObject);
        Debug.Log("Bullet Destroyed - Hit Enemy");
    }

    protected virtual void HitPlayer(Player player)
    {
        hasHit = true;
        player.takeDamage(damage); // Directly use the Player reference
        Destroy(gameObject);
        Debug.Log("Bullet Destroyed - Hit Player");
    }

    protected virtual void HitWall(Collider2D wall)
    {
        Destroy(gameObject);
        Debug.Log("Bullet Destroyed - Hit Wall");
    }

    public void SetDirection(Vector2 direction)
    {
        bulletDirection = direction;
    }

    public void SetGun(Gun gunReference)
    {
        gun = gunReference;
    }

    public void SetSpeed(float bulletSpeed)
    {
        speed = bulletSpeed;
    }

    public void SetShooter(ShooterType shooterType)
    {
        shooter = shooterType;
    }

    public void SetDamage(float bulletDamage)
    {
        damage = bulletDamage;
    }
}