using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildCardGun : Gun
{
    [Header("WildCardGunStats")]
    public List<Bullet> bulletPrefabs = new List<Bullet>();
    public int minDamage = 10;
    public int maxDamage = 20;
    public float minBulletSpeed = 5f;
    public float maxBulletSpeed = 15f;
    public float minCooldownTime = 0.1f;
    public float maxCooldownTime = 1f;
    [Header("ExplosiveBulletStats")]
    private int explosiveDamage;
    public int minExplosiveDamage = 50;
    public int maxExplosiveDamage = 100;
    private float explosiveRadius;
    public float minExplosiveRadius = 5f;
    public float maxExplosiveRadius = 10f;
    [Header("ShatterBulletStats")]
    private int shatterNumberOfBullets;
    public int minShatterNumberOfBullets = 5;
    public int maxShatterNumberOfBullets = 10;
    private float shatterAngleOfBulletSpread;
    public float minShatterAngleOfBulletSpread = 30f;
    public float maxShatterAngleOfBulletSpread = 60f;
    [Header("BouncingBulletStats")]
    private int bouncingBulletBounceCount;
    public int minBouncingBulletBounceCount = 3;
    public int maxBouncingBulletBounceCount = 5;
    public float bouncingBulletBounceDelay = 0.5f;
    public bool bouncingBulletBouncesOffEntities = false;
    [Header("PiercingBulletStats")]
    private int piercingBulletPierceCount;
    public int minPiercingBulletPierceCount = 1;
    public int maxPiercingBulletPierceCount = 5;

    void Awake()
    {
        SetBullet();
    }

    public override void Shoot(Vector3 aimTargetPostion)
    {
        base.Shoot(aimTargetPostion);
        SetBullet();
    }

    public override void SetBulletStats(Bullet bulletScript, Vector2 shootDirection)
    {
        GetRandomStats(bulletScript);
        base.SetBulletStats(bulletScript, shootDirection);
        if (bulletScript is ExplosiveBullet explosiveBullet)
        {
            explosiveBullet.SetExplosionDamage(explosiveDamage);
            explosiveBullet.SetExplosionRadius(explosiveRadius);
        }
        else if (bulletScript is ShatterBullet shatterBullet)
        {
            shatterBullet.SetNumberOfBullets(shatterNumberOfBullets);
            shatterBullet.SetAngleOfBulletSpread(shatterAngleOfBulletSpread);
            GameObject bulletShattered = bulletPrefabs[Random.Range(0, bulletPrefabs.Count)].gameObject;
            shatterBullet.SetBulletPrefab(bulletShattered);
            //Set the bullet shattered stats if its explosive/piercing/bouncing
        }
        else if (bulletScript is BouncingBullet bouncingBullet)
        {
            bouncingBullet.SetBounceCount(bouncingBulletBounceCount);
            bouncingBullet.SetBouncesOffEntities(bouncingBulletBouncesOffEntities);
            bouncingBullet.SetBounceDelay(bouncingBulletBounceDelay);
        }
        else if (bulletScript is PiercingBullet piercingBullet)
        {
            piercingBullet.SetPierceCount(piercingBulletPierceCount);
        }
    }

    private void SetBullet()
    {
        if (bulletPrefabs.Count == 0)
        {
            Debug.LogError("No bullet prefabs assigned to the WildCardGun.");
            return;
        }

        int randomIndex = Random.Range(0, bulletPrefabs.Count);
        bulletPrefab = bulletPrefabs[randomIndex].gameObject;
    }

    private void GetRandomStats(Bullet bullet)
    {
        damageStat = Random.Range(minDamage, maxDamage);
        bulletSpeed = Random.Range(minBulletSpeed, maxBulletSpeed);
        cooldownTime = Random.Range(minCooldownTime, maxCooldownTime);
        if (!(bullet is ExplosiveBullet) && !(bullet is ShatterBullet) && !(bullet is BouncingBullet) && !(bullet is PiercingBullet))
        {
            return;
        }

        if (bullet is ExplosiveBullet) 
        {
            explosiveDamage = Random.Range(minExplosiveDamage, maxExplosiveDamage);
            explosiveRadius = Random.Range(minExplosiveRadius, maxExplosiveRadius);
        }
        if (bullet is ShatterBullet)
        {
            shatterNumberOfBullets = Random.Range(minShatterNumberOfBullets, maxShatterNumberOfBullets);
            shatterAngleOfBulletSpread = Random.Range(minShatterAngleOfBulletSpread, maxShatterAngleOfBulletSpread);
        }
        if (bullet is BouncingBullet)
        {
            bouncingBulletBounceCount = Random.Range(minBouncingBulletBounceCount, maxBouncingBulletBounceCount);
            bouncingBulletBouncesOffEntities = Random.Range(0, 2) == 1;
        }
        if (bullet is PiercingBullet)
        {
            piercingBulletPierceCount = Random.Range(minPiercingBulletPierceCount, maxPiercingBulletPierceCount);
        }
    }
}
