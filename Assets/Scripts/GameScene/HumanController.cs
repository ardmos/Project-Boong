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

            // agent가 목적지를 계산하고 이동을 시작하기까지 잠시 기다려줍니다. 이 과정을 거치지 않으면 remainingDistance 계산이 올바로 되지 않습니다. 
            yield return new WaitForSeconds(1f);
            Debug.Log($"Human move start! movePoint: {point.gameObject.name}, remainingDistance: {agent.remainingDistance}");
            yield return new WaitUntil(() => agent.remainingDistance < 0.1f);
            Debug.Log($"Human move end! movePoint: {point.gameObject.name}, remainingDistance: {agent.remainingDistance}");
        }

        Debug.Log("MoveComplete!");
        // 모든 이동이 완료되면 콜백 함수 호출
        onAllMovementsComplete?.Invoke();
    }
}
