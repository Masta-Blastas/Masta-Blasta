using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Melee : EnemyBaseClass
{
    public void ChasePlayer()
    {
        state = State.Chasing;
    }
    
    public void Patrol()
    {
        state = State.Roaming;
    }
}
