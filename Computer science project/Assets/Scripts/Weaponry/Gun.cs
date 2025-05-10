using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Gun stats")]
    public string gunName;
    public string gunDescription;
    public int damageStat;
    public float cooldownTime = 0.5f;
    public float bulletSpeed = 1f;
    public float bulletLifetime = 10f; // Lifetime of the bullet in seconds
    [Header("Visual")]
    public float distanceFromPlayer = 1f;
    public SpriteRenderer gunSpriteRenderer;
    public Transform muzzlePoint;
    private Vector3 originalScale;
    public float scaleAmount = 1.2f;
    [Header("Audio")]
    public AudioClip shootSound;
    public AudioSource audioSource;
    [Header("Misc")]
    public GameObject bulletPrefab;
    protected float timeSinceLastShot = -Mathf.Infinity;

    public Transform ownerTransform;
    public Transform aimTarget;

    public enum GunShooterType
    {
        Player,
        Enemy
    }
    public GunShooterType gunShooterType;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
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

    public virtual void SetBulletStats(Bullet bulletScript, Vector2 shootDirection)
    {
        bulletScript.SetSpeed(bulletSpeed);
        bulletScript.SetDamage(damageStat);
        bulletScript.SetDirection(shootDirection);
        bulletScript.SetShooter(
            gunShooterType == GunShooterType.Player
            ? Bullet.ShooterType.Player
            : Bullet.ShooterType.Enemy
        );
        bulletScript.SetGun(this);
        bulletScript.SetBulletLifetime(bulletLifetime);
    }

    private IEnumerator StretchGunSprite()
    {
        gunSpriteRenderer.transform.localScale = originalScale; // Reset scale to original
        // Apply the stretching effect (recoil effect)
         // Randomize the scale amount
        gunSpriteRenderer.transform.localScale = new Vector3(originalScale.x * scaleAmount, originalScale.y / scaleAmount, originalScale.z);

        // Wait for the duration of the stretch effect
        yield return new WaitForSeconds(0.1f);

        // Reset the scale back to the original scale after the effect
        gunSpriteRenderer.transform.localScale = originalScale;
    }

    public virtual void ApplyShootEffect()
    {
        StartCoroutine(StretchGunSprite());

        float baseVolume = Random.Range(0.5f, 1.5f);
        float volumeMultiplier = gunShooterType == GunShooterType.Enemy ? 0.1f : 0.5f;
        float finalVolume = baseVolume * volumeMultiplier;

        audioSource.pitch = Random.Range(0.8f, 1.2f); // Slight pitch variation
        audioSource.PlayOneShot(shootSound, finalVolume);
    }
}
