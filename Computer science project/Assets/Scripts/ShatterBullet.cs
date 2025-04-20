using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatterBullet : Bullet
{
    public int bounceCount = 3; // Number of bounces before the bullet is destroyed
    //private int currentBounceCount = 0;
    public GameObject bulletPrefab;
    public int numberOfBullets;
    public float angleOfBulletSpread = 45f; // Angle spread for the bullet pieces
    private bool isReadyToShatter = false;
    public float armingDelay = 0.05f; // Delay before the bullet can shatter
    public int shatterGeneration;
    public int maxShatterGeneration = 3; // Maximum generation of shattering

    protected override void Start()
    {
        StartCoroutine(ArmDelay());
    }
    protected override void HitEnemy(Enemy enemy)
    {
        hasHit = true;
        enemy.takeDamage(damage); // Directly use the Enemy reference
        StartCoroutine(BreakIntoPieces(360f));
        Debug.Log("Bullet Destroyed - Hit Enemy");
    }

    protected override void HitPlayer(Player player)
    {        
        hasHit = true;
        player.takeDamage(damage); // Directly use the Player reference
        StartCoroutine(BreakIntoPieces(360f));
        Debug.Log("Bullet Destroyed - Hit Player");
    }

    protected override void HitWall(Collider2D collider)
    {        
        StartCoroutine(BreakIntoPieces(angleOfBulletSpread));
    }



    public IEnumerator BreakIntoPieces(float spreadAngle)
    {
        if (!isReadyToShatter) yield break; // Wait for the arming delay if not ready
        float newSpeed = speed; 
        float newDamage = Mathf.Round(damage * 0.7f); // Reduce the damage of the pieces

        if (numberOfBullets > 0 && shatterGeneration <= maxShatterGeneration)
        {
            for (int i = 0; i < numberOfBullets; i++)
            {
                Vector2 oppositeDir = -bulletDirection.normalized;

                float baseAngle = Mathf.Atan2(oppositeDir.y, oppositeDir.x) * Mathf.Rad2Deg;
                float randomAngle = baseAngle + Random.Range(-spreadAngle, spreadAngle);
                float rad = randomAngle * Mathf.Deg2Rad;
                Vector2 randomizedDir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;

                Vector2 spawnOffset = randomizedDir * 0.1f; // Offset to prevent overlap
                Vector2 spawnPosition = (Vector2)transform.position + spawnOffset;

                GameObject bulletPiece = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
                Bullet bulletPieceScript = bulletPiece.GetComponent<Bullet>();
                if (bulletPieceScript != null)
                {
                    bulletPieceScript.SetSpeed(newSpeed);
                    bulletPieceScript.SetDamage(newDamage);
                    bulletPieceScript.SetDirection(randomizedDir);
                    bulletPieceScript.SetShooter(shooter);
                    if (bulletPieceScript is ShatterBullet)
                    {
                        ShatterBullet shatterBulletPiece = (ShatterBullet)bulletPieceScript;
                        shatterBulletPiece.SetNumberOfBullets(3); //For lag issues, min 3
                        shatterBulletPiece.shatterGeneration = shatterGeneration + 1; // Increase the generation of the piece
                    }
                }
            }
            
        }
        else if (numberOfBullets <= 0)
        {
            // Handle the case when numberOfBullets is zero or less
            Debug.Log("Number of bullets is zero or less. Cannot shatter.");
        }
        yield return null;
        Destroy(gameObject);        
    }

    private IEnumerator ArmDelay()
    {
        yield return new WaitForSeconds(armingDelay);
        isReadyToShatter = true;
        yield return new WaitForSeconds(bulletLifetime);
        BreakIntoPieces(360);
    }
    public void SetNumberOfBullets(int number)
    {
        numberOfBullets = number;
    }
}



    /*private void ChangeDirection()
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
*/

//Code could be reused into another class idk tho, thunk on that