using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Melee : EnemyBaseClass
{

    public override void Start()
    {
        base.Start();

        state = State.Chasing; //sets default state to chasing. 
    }
}
