using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranger : Enemy
{
    [Header("Ranged Attack Settings")]
    public float minAttackRange = 3f; // Minimum range before backing away
    public float maxAttackRange = 6f; // Maximum range before moving closer
    private float lastShotTime = -1f;
    public float shootCooldown;
    public float baseShootCooldown;
    private Vector3 aimTarget;
    public enum AimType { Normal, Stormtrooper, Aimbot }
    public AimType aimType = AimType.Normal;
    [Header("Gun settings")]
    public Gun currentGun;
    public float accuracy = 0.5f; // Accuracy of the gun
    private Vector3 accuracyOffset;
    public List<Gun> gunList;

    protected override void Update()
    {
        aimTarget = playerTransform.position + accuracyOffset;
        base.Update();
        AttemptShoot();
        HandleMovement();
        HandleAiming();
    }

    private void HandleMovement()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer < minAttackRange)
        {
            RetreatFromPlayer();
        }
        else if (distanceToPlayer > maxAttackRange)
        {
            // Too far -> Move closer
            ChasePlayer();
        }
        else if (distanceToPlayer >= minAttackRange && distanceToPlayer <= maxAttackRange)
        {
            // In range -> Strafe
            StrafePlayer();
        }
    }

    private void AttemptShoot()
    {
        if (Time.time >= lastShotTime + shootCooldown)
        {
            Shoot();
            lastShotTime = Time.time;
        }
    }

    protected virtual void Shoot()
    {

        currentGun.Shoot(aimTarget);
        StartCoroutine(RerollAccuracy());
    }

    private void HandleAiming()
    {
        if (currentGun != null)
        {
            currentGun.RotateAndPositionGun(aimTarget);
        }
    }

    private IEnumerator RerollAccuracy()
    {
        yield return new WaitForSeconds(shootCooldown/2);
        if (aimType == AimType.Normal)
        {
            accuracyOffset = new Vector3(Random.Range(-accuracy, accuracy), Random.Range(-accuracy, accuracy), 0);
        }
        else if (aimType == AimType.Stormtrooper)
        {
            accuracyOffset = new Vector3(Random.Range(-accuracy, accuracy), Random.Range(-accuracy, accuracy), 0);
            accuracyOffset += new Vector3(Random.Range(-accuracy, accuracy), Random.Range(-accuracy, accuracy), 0);
        }
        else if (aimType == AimType.Aimbot)
        {
            accuracyOffset = Vector3.zero;
        }

    }


    private void SpawnGun()
    {
        Gun gunInstance = Instantiate(currentGun, this.transform);
        currentGun = gunInstance.GetComponent<Gun>();
        SetGunStats(currentGun);
        currentGun.ownerTransform = this.transform;
        currentGun.aimTarget = playerTransform;
    }

    private void SetGunStats(Gun currentGun)
    {
        currentGun.distanceFromPlayer = currentGun.distanceFromPlayer * 0.7f;
        currentGun.gunShooterType = Gun.GunShooterType.Enemy; // Set the gun shooter type to Enemy
    }
    private void SetGun()
    {
        if (gunList.Count == 0)
        {
            takeDamage(health);
            return;
        }
        if (gunList.Count > 1)
        {
            currentGun = gunList[Random.Range(0, gunList.Count)];
            return;
        }
        else if (gunList.Count == 1)
        {
            currentGun = gunList[0];
        }
    }

    private void SetAimType()
    {
        float randomValue = Random.Range(0f, 100f);
        if (randomValue < 20f)
        {
            aimType = AimType.Normal;
        }
        else if (randomValue < 99.9f)
        {
            aimType = AimType.Stormtrooper;
        }
        else
        {
            aimType = AimType.Aimbot;
        }
    }

    protected override void Initialise()
    {
        SetAimType();
        SetGun();
        base.Initialise();
        SpawnGun();
        shootCooldown = baseShootCooldown * GameManager.Instance.difficultyMultiplier;
    }
}