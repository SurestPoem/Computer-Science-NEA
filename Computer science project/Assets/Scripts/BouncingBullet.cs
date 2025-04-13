using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingBullet : Bullet
{
    public int bounceCount = 3; // Number of bounces before the bullet is destroyed
    private int currentBounceCount = 0;

    protected override void HitWall(Collider2D collider)
    {
        if (currentBounceCount < bounceCount)
        {
            ChangeDirection();
            currentBounceCount++;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void ChangeDirection()
    {
        Vector2 wallNormal = Vector2.zero;

        // If the collision is on the X axis (vertical wall), reflect the bullet on the X axis
        if (Mathf.Abs(GetComponent<Collider>().transform.position.x - transform.position.x) > Mathf.Abs(GetComponent<Collider>().transform.position.y - transform.position.y))
        {
            wallNormal = Vector2.right; // Reflect on the X-axis (vertical wall)
        }
        else
        {
            wallNormal = Vector2.up; // Reflect on the Y-axis (horizontal wall)
        }

        bulletDirection = Vector2.Reflect(bulletDirection, wallNormal); // Reflect the bullet direction
    }
}
