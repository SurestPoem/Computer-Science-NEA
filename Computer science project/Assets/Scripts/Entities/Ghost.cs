using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : Enemy
{
    void Update()
    {
        base.Update();
        ChasePlayer();
    }
}
