using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    [Header("Gun stats")]
    public string gunName;
    public string gunDescription;
    public float damageStat;
    public float cooldownTime = 0.5f;
    public float bulletSpeed = 1f;
    [Header("Visual")]
    public float distanceFromPlayer = 1f;
    public SpriteRenderer gunSpriteRenderer;
    public Transform muzzlePoint;
    private Vector3 originalScale;
    public float scaleAmount = 1.2f;
    [Header("Audio")]
    public AudioClip shootSound;
    [Header("Misc")]
    public GameObject bulletPrefab;
    protected float timeSinceLastShot = -Mathf.Infinity;

    public Transform ownerTransform;
    public Transform aimTarget;
    public Sprite gunIcon;

    void Start()
    {
        originalScale = gunSpriteRenderer.transform.localScale;
    }

    public virtual void RotateAndPositionGun(Vector3 aimTargetPosition)
    {
        aimTargetPosition.z = 0;

        Vector3 direction = aimTargetPosition - ownerTransform.position;
        direction.Normalize();

        transform.position = ownerTransform.position + direction * distanceFromPlayer;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        gunSpriteRenderer.flipY = direction.x < 0;
    }


    public virtual void Shoot()
    {
        if (Time.time - timeSinceLastShot < cooldownTime) // Time since last shot
            return;

        timeSinceLastShot = Time.time; // Record the time of this shot
        ApplyShootEffect(); // Apply shoot effect
        AudioManager.instance.PlaySound(shootSound, Random.Range(0.5f, 1.5f));

        // Get shoot direction based on crosshair world position
        Vector2 shootDirection = (aimTarget.position - muzzlePoint.position).normalized;

        // Create and initialize the bullet
        GameObject bullet = Instantiate(bulletPrefab, muzzlePoint.position, Quaternion.identity);
        Bullet bulletScript = bullet.GetComponent<Bullet>(); // Get the Bullet component

        

        if (bulletScript != null)
        {
            SetBulletStats(bulletScript, shootDirection); // Set the bullet stats
        }
    }

    public virtual void SetBulletStats(Bullet bulletScript, Vector2 shootDirection) // Set the bullet stats in the bullet script
    {
        bulletScript.SetSpeed(bulletSpeed);
        bulletScript.SetDamage(damageStat);
        bulletScript.SetDirection(shootDirection);
        bulletScript.SetShooter(Bullet.ShooterType.Player);
        bulletScript.SetGun(this);
    }

    private IEnumerator StretchGunSprite()
    {
        gunSpriteRenderer.transform.localScale = originalScale; // Reset scale to original
        // Apply the stretching effect (recoil effect)
         // Randomize the scale amount
        gunSpriteRenderer.transform.localScale = new Vector3(originalScale.x * scaleAmount, originalScale.y / scaleAmount, originalScale.z);

        // Wait for the duration of the stretch effect
        yield return new WaitForSeconds(0.15f);

        // Reset the scale back to the original scale after the effect
        gunSpriteRenderer.transform.localScale = originalScale;
    }

    public virtual void ApplyShootEffect()
    {
        // Stretch the sprite for recoil effect
        StartCoroutine(StretchGunSprite());
    }
}
