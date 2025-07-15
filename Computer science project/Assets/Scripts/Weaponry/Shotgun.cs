using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Gun
{
    [Header("Shotgun Specific")]
    public int numberOfPellets = 5;           // Number of pellets fired
    public float spreadAngle = 15f;           // Spread angle in degrees

    public override void Shoot(Vector3 aimTargetPostion)
    {
        if (Time.time - timeSinceLastShot < cooldownTime) // Time since last shot
            return;

        timeSinceLastShot = Time.time; // Record the time of this shot
        ApplyShootEffect(); // Apply shoot effect
        // Get the direction to the crosshair (target)
        Vector2 shootDirection = (aimTargetPostion - muzzlePoint.position).normalized;

        // Create multiple bullets with spread
        for (int i = 0; i < numberOfPellets; i++)
        {
            // Calculate the spread for each pellet
            float angleOffset = Random.Range(-spreadAngle / 2f, spreadAngle / 2f);

            // Rotate the shoot direction by the angle offset
            Vector2 pelletDirection = Quaternion.Euler(0, 0, angleOffset) * shootDirection;

            GameObject bullet = Instantiate(bulletPrefab, muzzlePoint.position, Quaternion.identity);
            Bullet bulletScript = bullet.GetComponent<Bullet>(); // Get the Bullet component

            if (bulletScript != null)
            {
                SetBulletStats(bulletScript, pelletDirection); // Set the bullet stats
            }
        }
    }
}