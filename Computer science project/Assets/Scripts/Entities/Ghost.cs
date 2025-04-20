using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : Enemy
{
    protected override void Update()
    {
        base.Update();
        ChasePlayer();
    }
}
