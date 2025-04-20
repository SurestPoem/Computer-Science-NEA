using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranger : Enemy
{
    [Header("Ranged Attack Settings")]
    public GameObject bulletPrefab; // Reference to bullet prefab
    public Transform firePoint; // Where bullets spawn
    public float minAttackRange = 3f; // Minimum range before backing away
    public float maxAttackRange = 6f; // Maximum range before moving closer
    private float lastShotTime = -1f;
    public float shootCooldown = 2f;
    public float baseShootCooldown;
    public float bulletSpeed = 1f;
    public float baseBulletSpeed;
    public float rangedDamage;
    public float baseRangedDamage;
    public int numberOfBullets = 1;
    public int baseNumberOfBullets = 1;
    public float spreadAngle = 45f; // degrees
    public float baseSpreadAngle = 45f; // degrees

    protected override void Update()
    {
        base.Update();
        AttemptShoot();
        HandleMovement();
    }

    private void HandleMovement()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer < minAttackRange)
        {
            RetreatFromPlayer();
        }
        else if (distanceToPlayer > maxAttackRange)
        {
            // Too far -> Move closer
            ChasePlayer();
        }
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
            // Calculate how many bullets to fire
            int bulletsToFire = Mathf.Max(numberOfBullets, baseNumberOfBullets);

            // Get the direction toward the player
            Vector2 direction = (playerTransform.position - firePoint.position).normalized;

            for (int i = 0; i < bulletsToFire; i++)
            {
                // Instantiate the bullet
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                Bullet bulletScript = bullet.GetComponent<Bullet>();

                if (bulletScript != null)
                {
                    bulletScript.SetDamage(rangedDamage);
                    bulletScript.SetSpeed(bulletSpeed);
                    bulletScript.SetShooter(Bullet.ShooterType.Enemy);

                    // If this is the first bullet, aim directly at the player
                    if (i == 0)
                    {
                        bulletScript.SetDirection(direction);
                    }
                    else
                    {
                        // Calculate spread for the other bullets
                        float angle = Random.Range(-spreadAngle / 2f, spreadAngle / 2f);
                        Vector2 spreadDirection = RotateDirection(direction, angle);
                        bulletScript.SetDirection(spreadDirection);
                    }
                }
            }
        }
    }

    // Utility method to rotate the direction vector by a given angle in degrees
    private Vector2 RotateDirection(Vector2 originalDirection, float angle)
    {
        float radians = angle * Mathf.Deg2Rad; // Convert angle to radians
        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);

        // Rotate the direction vector
        float x = originalDirection.x * cos - originalDirection.y * sin;
        float y = originalDirection.x * sin + originalDirection.y * cos;

        return new Vector2(x, y);
    }

    protected override void Initialise()
    {
        base.Initialise();
        shootCooldown = baseShootCooldown * GameManager.Instance.difficultyMultiplier;
        bulletSpeed = baseBulletSpeed * GameManager.Instance.difficultyMultiplier;
        rangedDamage = Mathf.RoundToInt(baseRangedDamage * GameManager.Instance.difficultyMultiplier);
        numberOfBullets = Mathf.RoundToInt(baseNumberOfBullets * GameManager.Instance.difficultyMultiplier);
        spreadAngle = baseSpreadAngle * GameManager.Instance.difficultyMultiplier;
    }
}