using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildCardGun : Gun
{
    public List<Bullet> bulletPrefabs = new List<Bullet>();

    private int explosiveDamage;
    public int minExplosiveDamage = 50;
    public int maxExplosiveDamage = 100;
    private float explosiveRadius;
    public float minExplosiveRadius = 5f;
    public float maxExplosiveRadius = 10f;

    private int shatterNumberOfBullets;
    public int minShatterNumberOfBullets = 5;
    public int maxShatterNumberOfBullets = 10;
    private float shatterAngleOfBulletSpread;
    public float minShatterAngleOfBulletSpread = 30f;
    public float maxShatterAngleOfBulletSpread = 60f;

    private int bouncingBulletBounceCount;
    public int minBouncingBulletBounceCount = 3;
    public int maxBouncingBulletBounceCount = 5;
    public float bouncingBulletBounceDelay = 0.5f;
    public bool bouncingBulletBouncesOffEntities = false;


    public override void Shoot()
    {
        base.Shoot();
        SetBullet();
    }

    public override void SetBulletStats(Bullet bulletScript, Vector2 shootDirection)
    {
        GetRandomStats();
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

    private void GetRandomStats()
    {
        explosiveDamage = Random.Range(minExplosiveDamage, maxExplosiveDamage);
        explosiveRadius = Random.Range(minExplosiveRadius, maxExplosiveRadius);
        shatterNumberOfBullets = Random.Range(minShatterNumberOfBullets, maxShatterNumberOfBullets);
        shatterAngleOfBulletSpread = Random.Range(minShatterAngleOfBulletSpread, maxShatterAngleOfBulletSpread);
        bouncingBulletBounceCount = Random.Range(minBouncingBulletBounceCount, maxBouncingBulletBounceCount);
        bouncingBulletBouncesOffEntities = Random.Range(0f, 1f) > 0.5f; // Randomly decide if it bounces off entities
    }
}
