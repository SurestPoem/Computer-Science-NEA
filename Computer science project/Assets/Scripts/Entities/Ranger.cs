using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranger : Enemy
{
    void Update()
    {
        base.Update();
        ChasePlayer();
    }
}
