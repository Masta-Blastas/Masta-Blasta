using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eyes : MonoBehaviour
{
    private Enemy_Melee enemyScript;

    private void Start()
    {
        enemyScript = GetComponentInParent<Enemy_Melee>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("oh shit!");
            //change state to chasing. 
            enemyScript.ChasePlayer();
        }
    }
}
