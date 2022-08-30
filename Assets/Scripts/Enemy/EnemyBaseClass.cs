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
    
    protected enum State
    {
        WaypointNav,
        Chasing,
        Attacking,
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
        }
        
    }

    public virtual void Attack()
    { 
        float distance = Vector3.Distance(transform.position, player.transform.position);
        transform.LookAt(player.transform);
        anim.SetBool("Walk", false);
        anim.SetBool("Attacking", true);
        _agent.isStopped = true;
        
        if (distance > 4)
        {
            state = State.Chasing;
        }
    }
    public virtual void Chasing() // base line logic for chase behavior
    {
        _agent.isStopped = false;
        anim.SetBool("Attacking", false);
        anim.SetBool("Walk", true);


        float distance = Vector3.Distance(transform.position, player.transform.position);
        currentTarget = 0;
  
        if(distance > 2)
        {
            _agent.SetDestination(playerPosition[currentTarget].position);
        }
        else if(distance <= 2)
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

                if(distance < 1.0f)
                {
                    anim.SetBool("Walk", false);
                }

                else if(distance > 1)
                {
                    anim.SetBool("Walk", true);
                }

                if (distance < 1.0f && targetReached == false)
                {
                    targetReached = true;

                    StartCoroutine(Idle());
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
}
