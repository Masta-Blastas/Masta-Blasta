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
    protected Transform playerPosition;
    [SerializeField]
    protected int currentTarget;
    protected bool reversing;
    [SerializeField]
    protected bool targetReached;

    [SerializeField]
    protected float range; // radius of a sphere\
    [SerializeField]
    protected Transform centerPoint; //center of the area the nav mesh agent wants to move within. 
    //if we dont care about the center point we can change this to the transform of the object. Should navigate randomly everywhere. 

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
        RandomNav,
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
        playerPosition = GameObject.Find("XR Rig").GetComponent<Transform>();
    }


    public virtual void Update()
    {
        switch(state)
        {
            case State.WaypointNav:
                WaypointNavigation();
                break;
            case State.RandomNav:
                RandomNavigation();
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

    #region RANDOM NAVIGATION ENEMIES
    public virtual void RandomNavigation()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);


        if (_agent.remainingDistance <= _agent.stoppingDistance) // done with path
        {

            Vector3 point;

            if (RandomPoint(centerPoint.position, range, out point))
            {
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                targetReached = true;
                _agent.SetDestination(point);
            }

            if(targetReached == true)
            {
                StartCoroutine(Scanning());
            }

        }

        if (distanceToPlayer <= 10)
        {
            state = State.Chasing;
        }
    }

    public virtual bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        NavMeshHit hit;

        if(NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero; 
        return false;
    }

    IEnumerator Scanning()
    {
        anim.SetBool("Scanning", true);
        _agent.isStopped = true ;
        yield return new WaitForSeconds(Random.Range(2.0f, 4.0f));
        _agent.isStopped = false;
        anim.SetBool("Scanning", false);
        targetReached = false;
    }
    #endregion

    #region CHASING BEHAVIOR
    public virtual void Chasing() // base line logic for chase behavior
    {
        _agent.isStopped = false;

        float distance = Vector3.Distance(transform.position, player.transform.position);
        currentTarget = 0; // current target is the player position

        if (distance > 7 && distance < 15) //if player is withing this distance, chase them
        {
            _agent.SetDestination(playerPosition.position);
        }
        else if (distance <= 7) // if closer, attack
        {
            state = State.Attacking;
        }

        if (distance >= 15) // if far away forget about player. 
        {
            state = State.RandomNav;
        }
    }

    #endregion

    #region ATTACK BEHAVIOR
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
    #endregion

    #region WAYPOINT NAVIGATION
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
    #endregion // probably delete could still use some of this for walk walkers.  // maybe useful for wall walkers


}
