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

    // �������� ������Ʈ�� ���� ���� ������ ����Ʈ��. ���ο� �� ���� ���� ������ ������ �߰��˴ϴ�.
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
        // �ִϸ��̼� ����
        SetAnimation(PuppyAnimationState.Idle);
    }

    private void Patrol()
    {
        // �ִϸ��̼� ����
        SetAnimation(PuppyAnimationState.Running);

        agent.speed = patrolSpeed;
        // ��Ʈ�� �������� �̵�
        MovePuppy(availablePatrolPoints[currentPatrolIndex].position);

        // ��Ʈ�� ���� ���� ���� Ȯ��
        if (Vector3.Distance(transform.position, availablePatrolPoints[currentPatrolIndex].position) < 0.1f)
        {
            // ������ ���� ��Ʈ�� ������ ����
            currentPatrolIndex = Random.Range(0, availablePatrolPoints.Count);
        }

        // chaseDistance ���� Player�� �ִ��� Ȯ��
        if (Vector3.Distance(transform.position, player.position) < chaseDistance)
        {
            SetPuppyState(PuppyBehaviourState.Chase);
            // Player, Audio���� �߰� ������ �˸�(Audio�� ���� �̱���)
            OnChasingStart.Invoke(this, EventArgs.Empty);
        }
    }

    private void Chase()
    {
        // �ִϸ��̼� ����
        SetAnimation(PuppyAnimationState.Running);

        agent.speed = chaseSpeed;
        // Player���� �̵�
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

    // �� �̺�Ʈ ó��
    private void OnDoorEvent(object sender, DoorEventArgs e)
    {
        // �� ���¿� ���� ó��
        switch (e.DoorEvent)
        {
            case DoorEvent.Opened:
                Debug.Log($"{e.DoorName}�� ���Ƚ��ϴ�.");
                // ���� ������ ���� ó��
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
                Debug.Log($"{e.DoorName}�� �������ϴ�.");
                // ���� ������ ���� ó��
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
