using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RichochetGun : Gun
{
    public bool bouncesOffEntities = false; // If true, the bullet will bounce off entities (enemies and players) as well
    public int bounceCount = 3; // Number of bounces before the bullet is destroyed
    public float bounceDelay;

    public override void SetBulletStats(Bullet bulletScript, Vector2 shootDirection)
    {
        base.SetBulletStats(bulletScript, shootDirection);
        if (bulletScript is BouncingBullet bouncingBullet)
        {
            bouncingBullet.SetBounceCount(bounceCount);
            bouncingBullet.SetBounceDelay(bounceDelay);
            bouncingBullet.SetBouncesOffEntities(bouncesOffEntities);
        }
    }
}