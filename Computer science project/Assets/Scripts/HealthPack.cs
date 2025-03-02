using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : Dropable
{
    public float healthValue;

    protected override void OnPickup()
    {
        player.Heal(healthValue);
    }


}
