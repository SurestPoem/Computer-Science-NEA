using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAimGun : Gun
{
    [Header("Auto-Aim Settings")]
    public float autoAimRadius = 10f;  // The radius in which to search for enemies
    public string enemyTag = "Enemy";  // The tag used for enemy objects

    public override void RotateAndPositionGun(Vector3 aimTargetPosition)
    {
        // Find all enemies within the specified radius
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(ownerTransform.position, autoAimRadius);

        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        // Loop through each enemy to find the closest one
        foreach (Collider2D enemyCollider in enemiesInRange)
        {
            if (enemyCollider.CompareTag(enemyTag))
            {
                float distanceToEnemy = Vector2.Distance(ownerTransform.position, enemyCollider.transform.position);

                // If this enemy is closer than the previous one, update the closest one
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = enemyCollider.transform;
                }
            }
        }

        // If no enemy is found, default to the crosshair position
        if (closestEnemy != null)
        {
            Vector3 targetPos = closestEnemy.position;
            targetPos.z = 0;

            Vector3 direction = targetPos - ownerTransform.position;
            direction.Normalize();

            transform.position = ownerTransform.position + direction * distanceFromPlayer;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            gunSpriteRenderer.flipY = direction.x < 0;
        }

        else
        {
            // No enemy found, aim at crosshair
            base.RotateAndPositionGun(aimTargetPosition);
        }
    }
}