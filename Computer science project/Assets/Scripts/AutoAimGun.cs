using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAimGun : Gun
{
    [Header("Auto-Aim Settings")]
    public string enemyTag = "Enemy";  // The tag used for enemy objects
    public float autoAimAssistStrength = 0.5f; // How much to nudge the aim towards the enemy

    public override void RotateAndPositionGun(Vector3 aimTargetPosition)
    {
        // Find the closest enemy in the general direction of the aim
        Transform closestEnemy = FindClosestEnemy();

        if (closestEnemy != null)
        {
            // Get the direction to the closest enemy
            Vector3 directionToEnemy = closestEnemy.position - ownerTransform.position;
            directionToEnemy.Normalize();

            // Apply a small nudge to the player's aim if the enemy is near the crosshair
            Vector3 direction = Vector3.Lerp(directionToEnemy, (aimTargetPosition - ownerTransform.position).normalized, autoAimAssistStrength);
            direction.Normalize();

            // Position and rotate the gun based on the nudge direction
            transform.position = ownerTransform.position + direction * distanceFromPlayer;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            // Flip the gun sprite based on the direction
            gunSpriteRenderer.flipY = direction.x < 0;
        }
        else
        {
            // No enemy found, default to the crosshair position
            base.RotateAndPositionGun(aimTargetPosition);
        }
    }

    private Transform FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            // Calculate the distance from the player
            float distance = Vector2.Distance(transform.position, enemy.transform.position);

            // Find the closest enemy
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy.transform;
            }
        }

        return closestEnemy;
    }

    public override void Shoot()
    {
        if (Time.time - timeSinceLastShot < cooldownTime) // Time since last shot
            return;

        timeSinceLastShot = Time.time; // Record the time of this shot
        ApplyShootEffect(); // Apply shoot effect

        // Get shoot direction based on crosshair world position
        Vector2 shootDirection = (muzzlePoint.position - ownerTransform.position).normalized;

        // Create and initialize the bullet
        GameObject bullet = Instantiate(bulletPrefab, muzzlePoint.position, Quaternion.identity);
        Bullet bulletScript = bullet.GetComponent<Bullet>(); // Get the Bullet component



        if (bulletScript != null)
        {
            SetBulletStats(bulletScript, shootDirection); // Set the bullet stats
        }
    }
}
