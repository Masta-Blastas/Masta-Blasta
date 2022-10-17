using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_BrainSnatcher : EnemyBaseClass
{
    [SerializeField]
    private GameObject explosionPrefab;

    public override void Start()
    {
        base.Start(); // gets necessary references
        state = State.RandomNav; 
    }

    public override void Update()
    {
        base.Update();
       
        if(HP <= 0)
        {
            scoreSystem.score.ApplyChange(pointValue); // applies value to score. 
            death.Invoke(); // invokes the death event
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject); // destroy. 
        }


    }

    public void StateChange_CHASE()
    {
        state = State.Chasing;
    }

    private void OnTriggerEnter(Collider other)
    {
        DamageDealer damage = other.GetComponent<DamageDealer>();

        if(damage != null) // if the thing with the collider has a damage dealer 
        {
            HP -= damage.damageAmount.Value; //takes away 30 HP 
        }
    }
}
