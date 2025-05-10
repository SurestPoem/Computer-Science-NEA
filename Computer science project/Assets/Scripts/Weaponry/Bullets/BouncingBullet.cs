using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingBullet : Bullet
{
    public int bounceCount = 3; // Number of bounces before the bullet is destroyed
    public float bounceDelay;
    public bool bouncesOffEntities = false; // If true, the bullet will bounce off entities (enemies and players) as well
    private int currentBounceCount = 0;
    private float lastBounceTime = 0f; // Time of the last bounce

    protected override void HitWall(Collider2D collider)
    {
        if (Time.time - lastBounceTime < bounceDelay)
            return; // too soon to bounce again
        if (currentBounceCount < bounceCount)
        {
            Bounce(); // Reflect the bullet direction
            lastBounceTime = Time.time;
        }
        else
        {
            base.HitWall(collider); // Call the base method to destroy the bullet
        }
    }

    protected override void HitEnemy(Enemy enemy)
    {
        if (bouncesOffEntities)
        {
            if (Time.time - lastBounceTime < bounceDelay)
                return; // too soon to bounce again
            if (currentBounceCount < bounceCount)
            {
                enemy.takeDamage(damage);
                Bounce(); // Reflect the bullet direction
                lastBounceTime = Time.time;
            }
            else
            {
                base.HitEnemy(enemy); // Call the base method to destroy the bullet
            }
        }
        else
        {
            base.HitEnemy(enemy);
        }
    }
    protected override void HitPlayer(Player player)
    {
        if (bouncesOffEntities)
        {
            if (Time.time - lastBounceTime < bounceDelay)
                return; // too soon to bounce again
            if (currentBounceCount < bounceCount)
            {
                player.takeDamage(damage);
                Bounce(); // Reflect the bullet direction
                lastBounceTime = Time.time;
            }
            else
            {
                base.HitPlayer(player); // Call the base method to destroy the bullet
            }
        }
        else
        {
            base.HitPlayer(player);
        }
    }
    private void Bounce()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -bulletDirection, 0.1f, LayerMask.GetMask("Wall"));

        if (hit.collider != null)
        {
            Vector2 normal = hit.normal;
            bulletDirection = Vector2.Reflect(bulletDirection, normal);
            currentBounceCount++;
        }
        else
        {
            // Fallback: just reverse X or Y if normal not found (basic bounce)
            bulletDirection = new Vector2(-bulletDirection.x, bulletDirection.y);
            currentBounceCount++;
        }
    }

    public void SetBounceCount(int count)
    {
        bounceCount = count;
    }
    public void SetBounceDelay(float delay)
    {
        bounceDelay = delay;
    }
    public void SetBouncesOffEntities(bool bounces)
    {
        bouncesOffEntities = bounces;
    }
}
