using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Blaster : EnemyBaseClass
{
    public override void Start()
    {
        base.Start();

        state = State.Patrol; // patrols until player enters their collider. 
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            state = State.Combat;
            
        }
    }
}
