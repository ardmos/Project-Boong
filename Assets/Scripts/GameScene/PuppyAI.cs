using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.AI;

public enum State
{
    Patrol,
    Chase
}

public class PuppyAI : MonoBehaviour
{
    public State currentState = State.Patrol;
    public Transform[] patrolPoints;
    public float chaseSpeed = 10f;
    public float patrolSpeed = 3f;
    public Transform player;
    public float chaseDistance = 10f;

    private int currentPatrolIndex = 0;

    [SerializeField] private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.Patrol:
                Patrol();
                break;
            case State.Chase:
                Chase();
                break;
        }
    }

    private void Patrol()
    {
        // Move towards the current patrol point
        //transform.position = Vector3.MoveTowards(transform.position, patrolPoints[currentPatrolIndex].position, patrolSpeed * Time.deltaTime);
        agent.speed = patrolSpeed;
        MovePuppy(patrolPoints[currentPatrolIndex].position);

        // Check if reached the patrol point
        if (Vector3.Distance(transform.position, patrolPoints[currentPatrolIndex].position) < 0.1f)
        {
            // Move to the next patrol point
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }

        // Check if the player is within chase distance
        if (Vector3.Distance(transform.position, player.position) < chaseDistance)
        {
            currentState = State.Chase;
        }
    }

    private void Chase()
    {
        // Move towards the player
        //transform.position = Vector3.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
        agent.speed = chaseSpeed;
        MovePuppy(player.position);

        // Check if the player is out of chase distance
        if (Vector3.Distance(transform.position, player.position) > chaseDistance)
        {
            currentState = State.Patrol;
        }
    }

    private void MovePuppy(Vector3 targetPos)
    {
        agent.SetDestination(targetPos);
    }
}
