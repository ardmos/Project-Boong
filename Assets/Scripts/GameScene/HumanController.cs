using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public enum HumanAnimations
{
    Idle,
    Walking
}

public class HumanController : MonoBehaviour
{
    public Transform[] movePointsIntroStep3;
    public Transform[] movePointsIntroStep4;
    public Transform[] movePointsGameOver;
    public Transform startPosition;

    private UnityAction onAllMovementsComplete;
    private Coroutine moveCoroutine;

    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        animator = GetComponentInChildren<Animator>();
    }

    private void MoveHuman(Vector3 targetPos)
    {
        Debug.Log($"targetPos:{targetPos}");
        agent.isStopped = false;
        agent.SetDestination(targetPos);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void ResetHuman()
    {
        if (moveCoroutine == null) return;
        if (!agent.isActiveAndEnabled) return;

        StopCoroutine(moveCoroutine);
        agent.isStopped = true;
        transform.position = startPosition.position;
        SetAnimation(HumanAnimations.Idle);
    }

    public void MoveToMovePointsIntroStep3(UnityAction onComplete)
    {
        onAllMovementsComplete = onComplete;
        moveCoroutine = StartCoroutine(MoveToPointSequentially(movePointsIntroStep3));
    }

    public void MoveToMovePointsIntroStep4(UnityAction onComplete)
    {
        onAllMovementsComplete = onComplete;
        moveCoroutine = StartCoroutine(MoveToPointSequentially(movePointsIntroStep4));
    }

    public void MoveToMovePointsGameOver(UnityAction onComplete)
    {
        onAllMovementsComplete = onComplete;
        moveCoroutine = StartCoroutine(MoveToPointSequentially(movePointsGameOver));
    }

    private void SetAnimation(HumanAnimations animation)
    {
        for (int i = (int)HumanAnimations.Idle; i <= (int)HumanAnimations.Walking; i++)
        {
            animator.SetBool($"{((HumanAnimations)i).ToString()}", false);
        }
        animator.SetBool(animation.ToString(), true);
    }

    private IEnumerator MoveToPointSequentially(Transform[] movePoints)
    {
        foreach (Transform point in movePoints)
        {
            MoveHuman(point.position);
            //agent.SetDestination(point.position);
            SetAnimation(HumanAnimations.Walking);
            // agent�� �������� ����ϰ� �̵��� �����ϱ���� ��� ��ٷ��ݴϴ�. �� ������ ��ġ�� ������ remainingDistance ����� �ùٷ� ���� �ʽ��ϴ�. 
            yield return new WaitForSeconds(1f);
            Debug.Log($"Human move start! movePoint: {point.gameObject.name}, remainingDistance: {agent.remainingDistance}");
            yield return new WaitUntil(() => agent.remainingDistance < 0.1f);
            Debug.Log($"Human move end! movePoint: {point.gameObject.name}, remainingDistance: {agent.remainingDistance}");
            SetAnimation(HumanAnimations.Idle);
        }

        Debug.Log("MoveComplete!");
        // ��� �̵��� �Ϸ�Ǹ� �ݹ� �Լ� ȣ��
        onAllMovementsComplete?.Invoke();
    }
}
