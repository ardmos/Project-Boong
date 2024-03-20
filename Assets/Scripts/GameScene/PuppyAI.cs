using System.Collections.Generic;
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

    // 동적으로 업데이트될 현재 접근 가능한 포인트들. 새로운 방 문이 열릴 때마다 영역이 추가됩니다.
    public List<Transform> availablePatrolPoints;

    public Transform[] patrolPointsLivingRoom;
    public Transform[] patrolPointsKitchen;
    public Transform[] patrolPointsBedRoom;
    public Transform[] patrolPointsBathRoom;
    public Transform[] patrolPointsWorkoutRoom;
    public Transform[] patrolPointsGarage;

    public float chaseSpeed = 10f;
    public float patrolSpeed = 3f;
    public Transform player;
    public float chaseDistance = 10f;

    [SerializeField] private int currentPatrolIndex = 0;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        animator = GetComponentInChildren<Animator>();

        availablePatrolPoints = new List<Transform>();
        availablePatrolPoints.AddRange(patrolPointsLivingRoom);

        currentState = State.Patrol;

        DoorManager.Instance.OnKitchenDoorOpen += OnKitchenDoorOpen;
        DoorManager.Instance.OnBedRoomDoorOpen += OnBedRoomDoorOpen;
        DoorManager.Instance.OnBathRoomDoorOpen += OnBathRoomDoorOpen;
        DoorManager.Instance.OnWorkoutRoomDoorOpen += OnWorkoutRoomDoorOpen;
        DoorManager.Instance.OnGarageDoorOpen += OnGarageDoorOpen;
    }

    private void OnGameOverTimeout(object sender, System.EventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void OnGarageDoorOpen(object sender, System.EventArgs e)
    {
        availablePatrolPoints.AddRange(patrolPointsGarage);
    }

    private void OnWorkoutRoomDoorOpen(object sender, System.EventArgs e)
    {
        availablePatrolPoints.AddRange(patrolPointsWorkoutRoom);
    }

    private void OnBathRoomDoorOpen(object sender, System.EventArgs e)
    {
        availablePatrolPoints.AddRange(patrolPointsBathRoom);
    }

    private void OnBedRoomDoorOpen(object sender, System.EventArgs e)
    {
        availablePatrolPoints.AddRange(patrolPointsBedRoom);
    }

    private void OnKitchenDoorOpen(object sender, System.EventArgs e)
    {
        availablePatrolPoints.AddRange(patrolPointsKitchen);
    }

    private void OnDisable()
    {
        if (DoorManager.Instance == null)
        {
            Debug.Log("OnDisable(): DoorManager.Instance is null");
            return;
        }
        DoorManager.Instance.OnKitchenDoorOpen -= OnKitchenDoorOpen;
        DoorManager.Instance.OnBedRoomDoorOpen -= OnBedRoomDoorOpen;
        DoorManager.Instance.OnBathRoomDoorOpen -= OnBathRoomDoorOpen;
        DoorManager.Instance.OnWorkoutRoomDoorOpen -= OnWorkoutRoomDoorOpen;
        DoorManager.Instance.OnGarageDoorOpen -= OnGarageDoorOpen;
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

        agent.speed = patrolSpeed;
        // Move towards the current patrol point
        MovePuppy(availablePatrolPoints[currentPatrolIndex].position);
        //Debug.Log($"currentPatrolIndex: {currentPatrolIndex}, availablePatrolPoints.Count: {availablePatrolPoints.Count}, next patrol point name: {availablePatrolPoints[currentPatrolIndex].name}");

        // Check if reached the patrol point
        if (Vector3.Distance(transform.position, availablePatrolPoints[currentPatrolIndex].position) < 0.1f)
        {
            //Debug.Log("patrol point reached! looking for next patrol point");
            // Move to the next patrol point            
            currentPatrolIndex = Random.Range(0, availablePatrolPoints.Count);
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

        agent.speed = chaseSpeed;
        // Move towards the player
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
