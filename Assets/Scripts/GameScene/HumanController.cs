using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class HumanController : MonoBehaviour
{
    public Transform[] movePointsIntroStep3;
    public Transform[] movePointsIntroStep4;
    public Transform[] movePointsGameOver;

    [SerializeField] private NavMeshAgent agent;
    private UnityAction onAllMovementsComplete;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void MoveHuman(Vector3 targetPos)
    {
        Debug.Log($"targetPos:{targetPos}");
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

    public void MoveToMovePointsIntroStep3(UnityAction onComplete)
    {
        onAllMovementsComplete = onComplete;
        StartCoroutine(MoveToPointSequentially(movePointsIntroStep3));
    }

    public void MoveToMovePointsIntroStep4(UnityAction onComplete)
    {
        onAllMovementsComplete = onComplete;
        StartCoroutine(MoveToPointSequentially(movePointsIntroStep4));
    }

    public void MoveToMovePointsGameOver(UnityAction onComplete)
    {
        onAllMovementsComplete = onComplete;
        StartCoroutine(MoveToPointSequentially(movePointsGameOver));
    }

    private IEnumerator MoveToPointSequentially(Transform[] movePoints)
    {
        foreach (Transform point in movePoints)
        {
            //MoveHuman(point.position);
            agent.SetDestination(point.position);

            // agent�� �������� ����ϰ� �̵��� �����ϱ���� ��� ��ٷ��ݴϴ�. �� ������ ��ġ�� ������ remainingDistance ����� �ùٷ� ���� �ʽ��ϴ�. 
            yield return new WaitForSeconds(1f);
            Debug.Log($"Human move start! movePoint: {point.gameObject.name}, remainingDistance: {agent.remainingDistance}");
            yield return new WaitUntil(() => agent.remainingDistance < 0.1f);
            Debug.Log($"Human move end! movePoint: {point.gameObject.name}, remainingDistance: {agent.remainingDistance}");
        }

        Debug.Log("MoveComplete!");
        // ��� �̵��� �Ϸ�Ǹ� �ݹ� �Լ� ȣ��
        onAllMovementsComplete?.Invoke();
    }
}
