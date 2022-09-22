using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_BrainSnatcher : EnemyBaseClass
{
    public override void Start()
    {
        base.Start(); // gets necessary references
        state = State.WaypointNav; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            state = State.Chasing;
        }
    }
}
