using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_BrainSnatcher_Eyes : MonoBehaviour
{
    private Enemy_BrainSnatcher enemy;

    private void Start()
    {
        enemy = GetComponentInParent<Enemy_BrainSnatcher>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Touched");
            enemy.StateChange_CHASE(); // accesses helper method to change state from base class. 
        }
    }
}
