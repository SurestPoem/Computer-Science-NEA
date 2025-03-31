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
    private bool hasHit = false;

    protected virtual void Start()
    {
        // Destroy the bullet after a set time (bulletLifetime)
        Destroy(gameObject, bulletLifetime);
    }

    void Update()
    {
        transform.Translate(bulletDirection * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        // If the bullet was fired by the player, it should damage enemies
        if (shooter == ShooterType.Player && collider.CompareTag("Enemy") && !hasHit)
        {
            hasHit = true;
            collider.GetComponent<Enemy>().takeDamage(gun.damageStat);
            Destroy(gameObject);
            Debug.Log("Bullet Destroyed - Hit Enemy");
        }

        // If the bullet was fired by an enemy, it should damage the player
        if (shooter == ShooterType.Enemy && collider.CompareTag("Player") && !hasHit)
        {
            hasHit = true;
            collider.GetComponent<Player>().takeDamage(gun.damageStat);
            Destroy(gameObject);
            Debug.Log("Bullet Destroyed - Hit Player");
        }

        // If the bullet hits a wall (TilemapCollider2D)
        if (collider.CompareTag("Wall") || collider.GetComponent<TilemapCollider2D>() != null)
        {
            HitWall(collider);
        }
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
}