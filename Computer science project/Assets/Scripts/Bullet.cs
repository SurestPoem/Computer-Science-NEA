using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bullet : MonoBehaviour
{
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
        // Move the bullet in the direction it was shot
        transform.Translate(bulletDirection * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        // If the bullet hits an enemy
        if (collider.CompareTag("Enemy") && !hasHit)
        {
            hasHit = true;
            collider.GetComponent<Enemy>().takeDamage(gun.damageStat);
            Destroy(gameObject);
            Debug.Log("Bullet Destroyed - Hit Enemy");
        }

        // If the bullet hits a wall (TilemapCollider2D)
        if (collider.CompareTag("Wall") || collider.GetComponent<TilemapCollider2D>() != null)
        {
            HitWall(collider); // Call the method that destroys or handles wall hit behavior
        }
    }

    // Method to handle what happens when bullet hits a wall
    protected virtual void HitWall(Collider2D wall)
    {
        Destroy(gameObject);
        Debug.Log("Bullet Destroyed - Hit Wall");
    }

    // Set the direction of the bullet
    public void SetDirection(Vector2 direction)
    {
        bulletDirection = direction;
    }

    // Set the Gun reference
    public void SetGun(Gun gunReference)
    {
        gun = gunReference;
    }

    public void SetSpeed(float bulletSpeed)
    {
        speed = bulletSpeed;
    }
}