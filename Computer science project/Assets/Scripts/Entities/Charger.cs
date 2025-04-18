using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charger : Enemy
{
    void Update()
    {
        base.Update();
        if (Time.time > lastDamageTime + damageCooldown)
        {
            // Enemy chases player after the damage cooldown ends
            ChasePlayer();
        }
        else
        {
           
        }
    }
}