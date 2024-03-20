using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CutSceneState
{
    Intro_Step1,
    Intro_Step2,
    Intro_Step3,
    Intro_Step4,
    Timeout,
    CaughtByPuppy,
    Win_Step1,
    Win_Step2
}

public class CutSceneManager : MonoBehaviour
{
    public static CutSceneManager Instance { get; private set; }

    public CutSceneState state;
    public CutSceneController cutSceneIntro;
    public CutSceneController cutSceneTimeout;
    public CutSceneController cutSceneCaughtByPuppy;
    public CutSceneController cutSceneWin;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameManager.Instance.OnGameOverTimeout += OnGameOverTimeout;
        GameManager.Instance.OnGameOverByPuppy += OnGameOverByPuppy;
        GameManager.Instance.OnGameWin += OnGameWin;
    }

    private void OnGameWin(object sender, System.EventArgs e)
    {
        cutSceneWin.ShowCutScene();
    }

    private void OnGameOverByPuppy(object sender, System.EventArgs e)
    {
        cutSceneCaughtByPuppy.ShowCutScene();
    }

    private void OnGameOverTimeout(object sender, System.EventArgs e)
    {
        cutSceneTimeout.ShowCutScene();
    }

    private void OnDisable()
    {
        if (GameManager.Instance == null)
        {
            Debug.Log("OnDisable(): GameManager.Instance is null");
            return;
        }

        GameManager.Instance.OnGameOverTimeout -= OnGameOverTimeout;
        GameManager.Instance.OnGameOverByPuppy -= OnGameOverByPuppy;
        GameManager.Instance.OnGameWin -= OnGameWin;
    }

    private void CutSceneStateMachine()
    {
        switch (state)
        {
            case CutSceneState.Intro_Step1:
                // �ƾ� 1 ����
                cutSceneIntro.ShowCutScene();
                StartCoroutine(WaitAndStartIntroStep2());
                break;
            case CutSceneState.Intro_Step2:
                // "����... �����...?" ���̾�α� ����
                DialogManager.Instance.ShowDialog("����... �����...?", () => {
                    cutSceneIntro.HideCutScene();
                    SetCutSceneState(CutSceneState.Intro_Step3);
                });
                break;
            case CutSceneState.Intro_Step3:
                // ��� �̵� �ִϸ��̼� ���. �ؾ ��Ź�� �ΰ� �����
                Debug.Log("Intro_Step3! "); 

                break;
            case CutSceneState.Intro_Step4:
                // "�̶���! ���� �����߰ھ�!" ���̾�α� ����

                break;
            case CutSceneState.Timeout:
                break;
            case CutSceneState.CaughtByPuppy:
                break;
            case CutSceneState.Win_Step1:
                break;
            case CutSceneState.Win_Step2:
                break;
            default:
                break;
        }
    }

    private void SetCutSceneState(CutSceneState cutSceneState)
    {
        state = cutSceneState;
        Debug.Log($"SetCutSceneState({cutSceneState}) called! new state is {state}");
        CutSceneStateMachine();
    }

    /// <summary>
    /// �ƾ� ����!
    /// </summary>
    public void StartCutScene()
    {
        SetCutSceneState(CutSceneState.Intro_Step1);
    }

    private IEnumerator WaitAndStartIntroStep2()
    {
        yield return new WaitForSeconds(1f);
        SetCutSceneState(CutSceneState.Intro_Step2);
    }
}
