using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    [Header("Gun stats")]
    public string gunName;
    public float damageStat;
    public float cooldownTime = 0.5f;
    public float bulletSpeed = 1f;
    [Header("Visual")]
    public float distanceFromPlayer = 1f;
    public SpriteRenderer gunSpriteRenderer;
    public Transform muzzlePoint;
    [Header("Misc")]
    public GameObject bulletPrefab;
    protected float timeSinceLastShot = 0f;
    
    public Transform playerTransform;
    public Transform crosshairTransform;
    public Sprite gunIcon;


    protected PlayerControls controls;

    void Start()
    {
        playerTransform = FindObjectOfType<Player>().transform;
        controls = new PlayerControls();
        controls.Player.Enable();  // Enable the controls

        GameObject crosshair = GameObject.FindGameObjectWithTag("Crosshair");

        if (crosshair != null)
        {
            crosshairTransform = crosshair.transform;
        }
        else
        {
            Debug.LogError("Crosshair not found");
        }
    }

    void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        RotateAndPositionGun(crosshairTransform.position);

    }

    public void RotateAndPositionGun(Vector3 crosshairPosition)
    {
        crosshairPosition.z = 0; // Keep it on the correct plane

        // Calculate direction from player to the crosshair
        Vector3 direction = crosshairPosition - playerTransform.position;
        direction.Normalize(); // Normalize to get direction only

        // Set the gun's position based on distance from the player
        transform.position = playerTransform.position + direction * distanceFromPlayer;

        // Calculate the angle between the gun and the crosshair
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Apply the rotation to the gun
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // Flip the gun sprite when moving left
        gunSpriteRenderer.flipY = direction.x < 0;
    }


    public virtual void Shoot()
    {
        if (timeSinceLastShot < cooldownTime)
            return;

        timeSinceLastShot = 0f;

        // Get shoot direction based on crosshair world position
        Vector2 shootDirection = (crosshairTransform.position - muzzlePoint.position).normalized;

        // Create and initialize the bullet
        GameObject bullet = Instantiate(bulletPrefab, muzzlePoint.position, Quaternion.identity);
        Bullet bulletScript = bullet.GetComponent<Bullet>(); // Get the Bullet component

        if (bulletScript != null)
        {
            bulletScript.SetSpeed(bulletSpeed);
            bulletScript.SetDamage(damageStat);
            bulletScript.SetDirection(shootDirection);
            bulletScript.SetShooter(Bullet.ShooterType.Player); 
            bulletScript.SetGun(this);  
        }
    }

}
