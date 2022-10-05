using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public abstract class EnemyBaseClass : MonoBehaviour
{
    //Making variables protected allows only classes that inherit from this class to manipulate them. 
    [SerializeField]
    protected float HP;
   
    [SerializeField]
    protected float pointValue;

    private Player player;

    public ScoreSystem scoreSystem;

    public UnityEvent death;

    //waypoints 
    [SerializeField]
    protected List<Transform> wayPoints; // can change size of a list at run time if we need to. Cannot change an array size. 
    [SerializeField]
    protected Transform[] playerPosition;
    [SerializeField]
    protected int currentTarget;
    protected bool reversing;
    protected bool targetReached;

    protected NavMeshAgent _agent;
    [SerializeField]
    protected Animator anim;

    [SerializeField]
    protected GameObject laserBeam;
    private bool attacking = false;

    
    protected enum State
    {
        //Brain Snatcher Methods
        WaypointNav,
        Chasing,
        Attacking,
        //Blaster Enemy States
        Patrol, 
        Combat,
        SeekShelter,
    }

    [SerializeField]
    protected State state;
   
    public virtual void Start()
    {
        player = GameObject.Find("XR Rig").GetComponent<Player>();
        _agent = GetComponent<NavMeshAgent>();
    }


    public virtual void Update()
    {
        switch(state)
        {
            case State.WaypointNav:
                WaypointNavigation();
                break;
            case State.Chasing:
                Chasing();
                break;
            case State.Attacking:
                Attack();
                break;
            case State.Patrol:
                WaypointNavigation(); // for now. 
                break;
            case State.Combat:
                CombatWaypoint();
                break;
        }
        
    }

    #region MELEE ENEMY FUNCTIONS
    public virtual void Attack()
    { 
        float distance = Vector3.Distance(transform.position, player.transform.position);
        transform.LookAt(player.transform);
        _agent.isStopped = true;

        if (attacking == false) //check if attacking is false to run this, otherwise it will just loop over and over. 
        {
            attacking = true;
            StartCoroutine(AttackLoop());
        }
        
        if (distance > 5)
        {
            state = State.Chasing;       //TO DO: Make it go back to weaypoint Nav when player is too far away/lost sight of player
            //StopCoroutine(AttackLoop());
        }
    }

    IEnumerator AttackLoop()
    {
        while(attacking == true)
        {
            yield return new WaitForSeconds(1.0f);
            laserBeam.SetActive(true);
            yield return new WaitForSeconds(3.0f);
            laserBeam.SetActive(false);
        }

        attacking = false; // set this back to false on the way out to make sure it is checked in Attack()
    }
    public virtual void Chasing() // base line logic for chase behavior
    {
        _agent.isStopped = false;

        float distance = Vector3.Distance(transform.position, player.transform.position);
        currentTarget = 0; // current target is the player position
        
        if (distance > 7)
        {
            _agent.SetDestination(playerPosition[currentTarget].position);
        }
        else if(distance <= 7)
        {
            state = State.Attacking;
        }
    }

    public virtual void WaypointNavigation()
    {      
        if (wayPoints.Count > 0) // are there waypoints?
        {
            if (wayPoints[currentTarget] != null)// does the current target exist?
            {
                _agent.SetDestination(wayPoints[currentTarget].position);
                

                float distance = Vector3.Distance(transform.position, wayPoints[currentTarget].position); // distance between target and enemy
                float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

                if (distance < 1.0f && targetReached == false)
                {
                    targetReached = true;
                                                                                                                                    //TO DO: set a looking back and forth animation to true. 
                    StartCoroutine(Idle());
                }

                if(distanceToPlayer <= 10)
                {
                    state = State.Chasing;
                }
            }
        }
    }

    IEnumerator Idle() // target reached is false, so resume the if statement in Patroling. 
    {
        yield return new WaitForSeconds(Random.Range(2.0f, 5.0f)); // pause for 2 - 5 seconds. 
        
        if(reversing == true)
        {
            currentTarget--;

            if (currentTarget == 0) // there are no more waypoints to decrement. 
            {
                reversing = false;
                currentTarget = 0; // set to zero
            }
        }

        else if(reversing == false)
        {
            currentTarget++;

            if (currentTarget == wayPoints.Count)  //if at the end of the waypoint list, reverse. 
            {
                //made it to the end. reverse
                reversing = true;
                currentTarget--;
            }
        }

        targetReached = false;
    }
    #endregion

    #region BLASTER ENEMY FUNCTIONS
    public virtual void CombatWaypoint()
    {
        transform.LookAt(player.transform.position);

        if (wayPoints.Count > 0) // are there waypoints?
        {
            if (wayPoints[currentTarget] != null)// does the current target exist?
            {
                _agent.SetDestination(wayPoints[currentTarget].position);


                float distance = Vector3.Distance(transform.position, wayPoints[currentTarget].position); // distance between target and enemy

                if (distance < 1.0f)
                {
                    anim.SetBool("Walk", false);
                    anim.SetBool("Firing", true);
                }

                else if (distance > 1)
                {
                    anim.SetBool("Walk", true);
                    anim.SetBool("Firing", false);

                }

                if (distance < 1.0f && targetReached == false)
                {
                    targetReached = true;

                    StartCoroutine(StopFireWeapon());
                }
            }
        }
    }

    IEnumerator StopFireWeapon()
    {
        yield return new WaitForSeconds(Random.Range(6.0f, 8.0f)); // pause for 2 - 5 seconds. 

        if (reversing == true)
        {
            currentTarget--;

            if (currentTarget == 0) // there are no more waypoints to decrement. 
            {
                reversing = false;
                currentTarget = 0; // set to zero
            }
        }

        else if (reversing == false)
        {
            currentTarget++;

            if (currentTarget == wayPoints.Count)  //if at the end of the waypoint list, reverse. 
            {
                //made it to the end. reverse
                reversing = true;
                currentTarget--;
            }
        }

        targetReached = false;
    }
    #endregion
}
