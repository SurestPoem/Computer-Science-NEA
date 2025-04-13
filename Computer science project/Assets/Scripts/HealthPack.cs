using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : Dropable
{
    public float healthValue;

    protected override void OnPickup()
    {
        Debug.Log("Health Pack Picked Up, value: " + healthValue);
        player.Heal(healthValue);
    }


}
