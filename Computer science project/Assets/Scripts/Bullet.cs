using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float bulletLifetime;
    public Vector2 bulletDirection;
    private Gun gun;  // Reference to the Gun
    private bool hasHit = false;

    void Start()
    {
        // Destroy the bullet after a set time (bulletLifetime)
        Destroy(gameObject, bulletLifetime);

    }

    void Update()
    {
        // Move the bullet in the direction it was shot
        transform.Translate(bulletDirection * speed * Time.deltaTime);
    }

    // Detect collision with enemy
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy") && hasHit == false)
        {
            hasHit = true;
            // Access the Gun's damageStat, as the Bullet has the Gun reference
            collider.GetComponent<Enemy>().takeDamage(gun.damageStat);
            Destroy(gameObject);
            Debug.Log("Bullet Destroyed - Hit");
        }
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