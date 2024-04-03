using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public enum PuppyBehaviourState
{
    Idle,
    Patrol,
    Chase
}

public enum PuppyAnimationState
{
    Idle,
    Running,
    Eating
}

public class PuppyAI : MonoBehaviour
{
    public static PuppyAI Instance { get; private set; }

    public PuppyBehaviourState currentState;

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
        DoorManager.Instance.SubscribeToDoorEvent(DoorName.Door_Kitchen, OnDoorEvent);
        DoorManager.Instance.SubscribeToDoorEvent(DoorName.Door_BedRoom, OnDoorEvent);
        DoorManager.Instance.SubscribeToDoorEvent(DoorName.Door_BathRoom, OnDoorEvent);
        DoorManager.Instance.SubscribeToDoorEvent(DoorName.Door_WorkoutRoom, OnDoorEvent);
        DoorManager.Instance.SubscribeToDoorEvent(DoorName.Door_Garage, OnDoorEvent);
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
        DoorManager.Instance.UnsubscribeFromDoorEvent(DoorName.Door_Kitchen, OnDoorEvent);
        DoorManager.Instance.UnsubscribeFromDoorEvent(DoorName.Door_BedRoom, OnDoorEvent);
        DoorManager.Instance.UnsubscribeFromDoorEvent(DoorName.Door_BathRoom, OnDoorEvent);
        DoorManager.Instance.UnsubscribeFromDoorEvent(DoorName.Door_WorkoutRoom, OnDoorEvent);
        DoorManager.Instance.UnsubscribeFromDoorEvent(DoorName.Door_Garage, OnDoorEvent);
    }

    public void SetPuppyState(PuppyBehaviourState state)
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
        SetPuppyState(PuppyBehaviourState.Idle);
    }

    private void PuppyStateMachine()
    {
        switch (currentState)
        {
            case PuppyBehaviourState.Idle:
                Idle();
                break;
            case PuppyBehaviourState.Patrol:
                Patrol();
                break;
            case PuppyBehaviourState.Chase:
                Chase();
                break;
        }
    }

    private void Idle()
    {
        // 애니메이션 실행
        SetAnimation(PuppyAnimationState.Idle);
    }

    private void Patrol()
    {
        // 애니메이션 실행
        SetAnimation(PuppyAnimationState.Running);

        agent.speed = patrolSpeed;
        // 패트롤 지점으로 이동
        MovePuppy(availablePatrolPoints[currentPatrolIndex].position);

        // 패트롤 지점 도착 여부 확인
        if (Vector3.Distance(transform.position, availablePatrolPoints[currentPatrolIndex].position) < 0.1f)
        {
            // 도착시 다음 패트롤 지점을 변경
            currentPatrolIndex = Random.Range(0, availablePatrolPoints.Count);
        }

        // chaseDistance 내에 Player가 있는지 확인
        if (Vector3.Distance(transform.position, player.position) < chaseDistance)
        {
            SetPuppyState(PuppyBehaviourState.Chase);
            // Player, Audio에게 추격 시작을 알림(Audio는 아직 미구현)
            OnChasingStart.Invoke(this, EventArgs.Empty);
        }
    }

    private void Chase()
    {
        // 애니메이션 실행
        SetAnimation(PuppyAnimationState.Running);

        agent.speed = chaseSpeed;
        // Player에게 이동
        MovePuppy(player.position);

        // Check if the player is out of chase distance
        if (Vector3.Distance(transform.position, player.position) > chaseDistance)
        {
            SetPuppyState(PuppyBehaviourState.Patrol);
        }
    }

    private void MovePuppy(Vector3 targetPos)
    {
        agent.isStopped = false;
        agent.SetDestination(targetPos);
    }

    private void SetAnimation(PuppyAnimationState animation)
    {
        if (animator.GetBool(animation.ToString())) return;

        for (int i = (int)PuppyAnimationState.Idle; i <= (int)PuppyAnimationState.Eating; i++)
        {
            animator.SetBool($"{((PuppyAnimationState)i).ToString()}", false);
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
                    case DoorName.Door_Kitchen:
                        availablePatrolPoints.AddRange(patrolPointsKitchen);
                        break;
                    case DoorName.Door_BedRoom:
                        availablePatrolPoints.AddRange(patrolPointsBedRoom);
                        break;
                    case DoorName.Door_BathRoom:
                        availablePatrolPoints.AddRange(patrolPointsBathRoom);
                        break;
                    case DoorName.Door_WorkoutRoom:
                        availablePatrolPoints.AddRange(patrolPointsWorkoutRoom);
                        break;
                    case DoorName.Door_Garage:
                        availablePatrolPoints.AddRange(patrolPointsGarage);
                        break;
                }
                break;
            case DoorEvent.Closed:
                Debug.Log($"{e.DoorName}이 닫혔습니다.");
                // 문이 닫혔을 때의 처리
                switch (e.DoorName)
                {
                    case DoorName.Door_Kitchen:
                        availablePatrolPoints.RemoveAll(point => Array.Exists(patrolPointsKitchen, p => p == point));
                        break;
                    case DoorName.Door_BedRoom:
                        availablePatrolPoints.RemoveAll(point => Array.Exists(patrolPointsBedRoom, p => p == point));
                        break;
                    case DoorName.Door_BathRoom:
                        availablePatrolPoints.RemoveAll(point => Array.Exists(patrolPointsBathRoom, p => p == point));
                        break;
                    case DoorName.Door_WorkoutRoom:
                        availablePatrolPoints.RemoveAll(point => Array.Exists(patrolPointsWorkoutRoom, p => p == point));
                        break;
                    case DoorName.Door_Garage:
                        availablePatrolPoints.RemoveAll(point => Array.Exists(patrolPointsGarage, p => p == point));
                        break;
                }
                break;
        }
    }
}
