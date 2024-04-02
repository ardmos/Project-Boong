using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public enum PuppyState
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
    public static PuppyAI Instance { get; private set; }

    public PuppyState currentState;

    // 동적으로 업데이트될 현재 접근 가능한 포인트들. 새로운 방 문이 열릴 때마다 영역이 추가됩니다.
    public List<Transform> availablePatrolPoints;

    public Transform[] patrolPointsLivingRoom;
    public Transform[] patrolPointsKitchen;
    public Transform[] patrolPointsBedRoom;
    public Transform[] patrolPointsBathRoom;
    public Transform[] patrolPointsWorkoutRoom;
    public Transform[] patrolPointsGarage;

    public Transform startPosition;

    public event EventHandler OnChasingStart;

    public float chaseSpeed = 10f;
    public float patrolSpeed = 3f;
    public Transform player;
    public float chaseDistance = 10f;

    private int currentPatrolIndex = 0;
    private NavMeshAgent agent;
    private Animator animator;

    private void Awake()
    {
        Instance = this;

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        animator = GetComponentInChildren<Animator>();

        availablePatrolPoints = new List<Transform>();
        availablePatrolPoints.AddRange(patrolPointsLivingRoom);
    }

    private void Start()
    {
        // Add Callbacks
        DoorManager.Instance.SubscribeToDoorEvent(DoorNames.Door_Kitchen, OnDoorEvent);
        DoorManager.Instance.SubscribeToDoorEvent(DoorNames.Door_BedRoom, OnDoorEvent);
        DoorManager.Instance.SubscribeToDoorEvent(DoorNames.Door_BathRoom, OnDoorEvent);
        DoorManager.Instance.SubscribeToDoorEvent(DoorNames.Door_WorkoutRoom, OnDoorEvent);
        DoorManager.Instance.SubscribeToDoorEvent(DoorNames.Door_Garage, OnDoorEvent);
    }

    private void Update()
    {
        PuppyStateMachine();
    }

    private void OnDisable()
    {
        if (DoorManager.Instance == null)
        {
            Debug.Log("OnDisable(): DoorManager.Instance is null");
            return;
        }
        // Unregister Callbacks
        DoorManager.Instance.UnsubscribeFromDoorEvent(DoorNames.Door_Kitchen, OnDoorEvent);
        DoorManager.Instance.UnsubscribeFromDoorEvent(DoorNames.Door_BedRoom, OnDoorEvent);
        DoorManager.Instance.UnsubscribeFromDoorEvent(DoorNames.Door_BathRoom, OnDoorEvent);
        DoorManager.Instance.UnsubscribeFromDoorEvent(DoorNames.Door_WorkoutRoom, OnDoorEvent);
        DoorManager.Instance.UnsubscribeFromDoorEvent(DoorNames.Door_Garage, OnDoorEvent);
    }

    public void SetPuppyState(PuppyState state)
    {
        currentState = state;
    }

    public void ResetPuppy()
    {
        if (!agent.isActiveAndEnabled) return;

        agent.isStopped = true;
        agent.enabled = false;
        transform.position = startPosition.position;
        agent.enabled = true;
        SetPuppyState(PuppyState.Idle);
    }

    private void PuppyStateMachine()
    {
        switch (currentState)
        {
            case PuppyState.Idle:
                Idle();
                break;
            case PuppyState.Patrol:
                Patrol();
                break;
            case PuppyState.Chase:
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

        // Check if reached the patrol point
        if (Vector3.Distance(transform.position, availablePatrolPoints[currentPatrolIndex].position) < 0.1f)
        {
            // Move to the next patrol point            
            currentPatrolIndex = Random.Range(0, availablePatrolPoints.Count);
        }

        // Check if the player is within chase distance
        if (Vector3.Distance(transform.position, player.position) < chaseDistance)
        {
            SetPuppyState(PuppyState.Chase);
            // Player, Audio에게 추격 시작을 알림(Audio는 아직 미구현)
            OnChasingStart.Invoke(this, EventArgs.Empty);
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
            SetPuppyState(PuppyState.Patrol);
        }
    }

    private void MovePuppy(Vector3 targetPos)
    {
        agent.isStopped = false;
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

    // 문 이벤트 처리
    private void OnDoorEvent(object sender, DoorEventArgs e)
    {
        // 문 상태에 따라 처리
        switch (e.DoorEvent)
        {
            case DoorEvent.Opened:
                Debug.Log($"{e.DoorName}이 열렸습니다.");
                // 문이 열렸을 때의 처리
                switch (e.DoorName)
                {
                    case DoorNames.Door_Kitchen:
                        availablePatrolPoints.AddRange(patrolPointsKitchen);
                        break;
                    case DoorNames.Door_BedRoom:
                        availablePatrolPoints.AddRange(patrolPointsBedRoom);
                        break;
                    case DoorNames.Door_BathRoom:
                        availablePatrolPoints.AddRange(patrolPointsBathRoom);
                        break;
                    case DoorNames.Door_WorkoutRoom:
                        availablePatrolPoints.AddRange(patrolPointsWorkoutRoom);
                        break;
                    case DoorNames.Door_Garage:
                        availablePatrolPoints.AddRange(patrolPointsGarage);
                        break;
                }
                break;
            case DoorEvent.Closed:
                Debug.Log($"{e.DoorName}이 닫혔습니다.");
                // 문이 닫혔을 때의 처리
                switch (e.DoorName)
                {
                    case DoorNames.Door_Kitchen:
                        availablePatrolPoints.RemoveAll(point => Array.Exists(patrolPointsKitchen, p => p == point));
                        break;
                    case DoorNames.Door_BedRoom:
                        availablePatrolPoints.RemoveAll(point => Array.Exists(patrolPointsBedRoom, p => p == point));
                        break;
                    case DoorNames.Door_BathRoom:
                        availablePatrolPoints.RemoveAll(point => Array.Exists(patrolPointsBathRoom, p => p == point));
                        break;
                    case DoorNames.Door_WorkoutRoom:
                        availablePatrolPoints.RemoveAll(point => Array.Exists(patrolPointsWorkoutRoom, p => p == point));
                        break;
                    case DoorNames.Door_Garage:
                        availablePatrolPoints.RemoveAll(point => Array.Exists(patrolPointsGarage, p => p == point));
                        break;
                }
                break;
        }
    }
}
