using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{


    // Update is called once per frame
    void Update()
    {
        ChasePlayer();
        base.Update();
    }
}
