using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRanger : Ranger
{
    [Header("Gun settings")]
    public GameObject gun;
    public Gun currentGun;
    public float accuracy = 0.5f; // Accuracy of the gun

    protected override void Update()
    {
        base.Update();
        HandleAiming();
    }

    protected override void Shoot()
    {
        currentGun.Shoot();
    }

    private void HandleAiming()
    {
        if (currentGun != null)
        {
            // Calculate the aim target position based on the player's position and accuracy
            Vector3 aimTargetPosition = playerTransform.position;
            currentGun.RotateAndPositionGun(aimTargetPosition);
        }
    }

    private void SpawnGun()
    {
        GameObject gunInstance = Instantiate(gun, this.transform);
        currentGun = gunInstance.GetComponent<Gun>();
        SetGunStats(currentGun);
        currentGun.ownerTransform = this.transform;
        currentGun.aimTarget = playerTransform;
    }

    private void SetGunStats(Gun currentGun)
    {
        currentGun.cooldownTime = shootCooldown;
        currentGun.bulletSpeed = bulletSpeed;
        currentGun.damageStat = rangedDamage;
        currentGun.distanceFromPlayer = currentGun.distanceFromPlayer * 0.7f;
        currentGun.gunShooterType = Gun.GunShooterType.Enemy; // Set the gun shooter type to Enemy
    }
    protected override void Initialise()
    {
        base.Initialise();
        SpawnGun();
    }
}
