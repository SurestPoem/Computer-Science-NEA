using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBullet : Bullet
{
    public float explosionRadius = 5f;
    public float explosionDamage = 50f;
    public GameObject explosionEffect;
    // Start is called before the first frame update
    

    protected override void HitEnemy(Enemy enemy)
    {
        hasHit = true;
        enemy.takeDamage(damage); // Directly use the Enemy reference
        Explode();
    }

    protected override void HitPlayer(Player player)
    {
        hasHit = true;
        player.takeDamage(damage); // Directly use the Player reference
        Explode();
    }

    protected override void HitWall(Collider2D wall)
    {
        Explode();
    }

    private void Explode()
    {
        // Create explosion effect
        if (explosionEffect != null)
        {
            GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            float scaleFactor = explosionRadius * 2;  // Adjust the divisor to control scaling factor
            explosion.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
            Destroy(explosion, 1f); // Destroy the explosion effect after 2 seconds
        }
        // Find all enemies within the explosion radius and apply damage
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        int damagedPerExplosion = 0;
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy") && (shooter == ShooterType.Player || shooter == ShooterType.None))
            {
                collider.GetComponent<Enemy>().takeDamage(explosionDamage);
                damagedPerExplosion++;
            }
            else if (collider.CompareTag("Player") && (shooter == ShooterType.Enemy || shooter == ShooterType.None))
            {
                collider.GetComponent<Player>().takeDamage(explosionDamage);
                damagedPerExplosion++;
            }
        }
        Destroy(gameObject);
        Debug.Log("Entities damaged by explosion: " + damagedPerExplosion);
    }

    public void SetExplosionRadius(float radius)
    {
        explosionRadius = radius;
    }

    public void SetExplosionDamage(float damage)
    {
        explosionDamage = damage;
    }
}
