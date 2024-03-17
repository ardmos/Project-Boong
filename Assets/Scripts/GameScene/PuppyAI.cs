using UnityEngine;
using UnityEngine.AI;

public enum State
{
    Idle,
    Patrol,
    Chase
}

public enum CharacterAnimations
{
    Idle,
    Running,
    Eating
}

public class PuppyAI : MonoBehaviour
{
    public State currentState;
    public Transform[] patrolPoints;
    public float chaseSpeed = 10f;
    public float patrolSpeed = 3f;
    public Transform player;
    public float chaseDistance = 10f;

    private int currentPatrolIndex = 0;

    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        animator = GetComponentInChildren<Animator>();

        // 테스트를 위한 처리.
        //currentState = State.Patrol;

        GameManager.Instance.OnGameEnd += OnGameEnd;
    }

    private void OnDisable()
    {
        if (GameManager.Instance == null)
        {
            Debug.Log("OnDisable(): GameManager.Instance is null");
            return;
        }
        GameManager.Instance.OnGameEnd -= OnGameEnd;
    }

    private void OnGameEnd(object sender, System.EventArgs e)
    {
        currentState = State.Idle;
    }

    private void Update()
    {
        PuppyStateMachine();
    }

    private void PuppyStateMachine()
    {
        switch (currentState)
        {
            case State.Idle:
                Idle();
                break;
            case State.Patrol:
                Patrol();
                break;
            case State.Chase:
                Chase();
                break;
        }
    }

    private void Idle()
    {
        // Set animation
        SetAnimation(CharacterAnimations.Idle);
    }

    private void Patrol()
    {
        // Set animation
        SetAnimation(CharacterAnimations.Running);

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
        // Set animation
        SetAnimation(CharacterAnimations.Running);

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

    private void SetAnimation(CharacterAnimations animation)
    {
        if (animator.GetBool(animation.ToString())) return;

        for (int i = (int)CharacterAnimations.Idle; i <= (int)CharacterAnimations.Eating; i++)
        {
            animator.SetBool($"{((CharacterAnimations)i).ToString()}", false);           
        }

        animator.SetBool(animation.ToString(), true);
    }
}
