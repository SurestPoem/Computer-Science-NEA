using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveGun : Gun
{
    public float explosionRadius = 5f;
    public float explosionDamage = 50f;

    public override void SetBulletStats(Bullet bulletScript, Vector2 shootDirection)
    {
        base.SetBulletStats(bulletScript, shootDirection);

        if (bulletScript is ExplosiveBullet explosiveBullet)
        {
            explosiveBullet.SetExplosionRadius(explosionRadius);
            explosiveBullet.SetExplosionDamage(explosionDamage);
        }
    }
}
