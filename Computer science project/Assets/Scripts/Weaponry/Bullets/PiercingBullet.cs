using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiercingBullet : Bullet
{
    public int pierceCount = 3;
    private int currentPierceCount = 0;

    protected override void HitEnemy(Enemy enemy)
    {
        if (currentPierceCount < pierceCount)
        {
            currentPierceCount++;
            enemy.takeDamage(damage);
        }
        else
        {
            base.HitEnemy(enemy);
        }
    }

    protected override void HitPlayer(Player player)
    {
        if (currentPierceCount < pierceCount)
        {
            currentPierceCount++;
            player.takeDamage(damage);
        }
        else
        {
            base.HitPlayer(player);
        }
    }

    public void SetPierceCount(int count)
    {
        pierceCount = count;
    }
}
