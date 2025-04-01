using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranger : Enemy
{
    [Header("Ranged Attack Settings")]
    public GameObject bulletPrefab; // Reference to bullet prefab
    public Transform firePoint; // Where bullets spawn
    public float shootCooldown = 2f; // Time between shots
    public float minAttackRange = 3f; // Minimum range before backing away
    public float maxAttackRange = 6f; // Maximum range before moving closer
    private float lastShotTime = -1f;
    public float bulletSpeed = 1f;
    public float rangedDamage = 10f;

    void Update()
    {
        base.Update();
        HandleMovement();
        AttemptShoot();
    }

    private void HandleMovement()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer < minAttackRange)
        {
            // Too close -> Back away
            Vector3 direction = (transform.position - playerTransform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
        else if (distanceToPlayer > maxAttackRange)
        {
            // Too far -> Move closer
            ChasePlayer();
        }
        // If within range, stay in place and attack
    }

    private void AttemptShoot()
    {
        if (Time.time >= lastShotTime + shootCooldown)
        {
            Shoot();
            lastShotTime = Time.time;
        }
    }

    private void Shoot()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            Bullet bulletScript = bullet.GetComponent<Bullet>();

            if (bulletScript != null)
            {
                bulletScript.SetDamage(rangedDamage);
                Vector2 direction = (playerTransform.position - firePoint.position).normalized;
                bulletScript.SetDirection(direction);
                bulletScript.SetSpeed(bulletSpeed);
                bulletScript.SetShooter(Bullet.ShooterType.Enemy);
            }
        }
    }
}