using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Blaster : EnemyBaseClass
{
    public override void Start()
    {
        base.Start();

        state = State.WaypointNav;
    }

    public override void WaypointNavigation()
    {
        base.WaypointNavigation();
        transform.LookAt(playerPosition[0]);
    }
}
