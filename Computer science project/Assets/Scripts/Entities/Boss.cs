using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    public Bullet bullet;
    public float bulletSpeed = 5f;
    public float bulletDamage = 10f;



    private IEnumerator SpawnBulletsAroundPlayer()
    {
        return null;
    }
}
