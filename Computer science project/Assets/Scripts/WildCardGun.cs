using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildCardGun : Gun
{
    public List<Bullet> bulletPrefabs = new List<Bullet>();

    public int explosiveDamage = 50;
    public float explosiveRadius = 5f;

    public int shatterNumberOfBullets = 5;
    public float shatterAngleOfBulletSpread = 30f;

    public override void Shoot()
    {
        base.Shoot();
        SetBullet();
    }

    public override void SetBulletStats(Bullet bulletScript, Vector2 shootDirection)
    {
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
}
