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
                // 컷씬 1 노출
                cutSceneIntro.ShowCutScene();
                StartCoroutine(WaitAndStartIntroStep2());
                break;
            case CutSceneState.Intro_Step2:
                // "여긴... 어디지...?" 다이얼로그 노출
                DialogManager.Instance.ShowDialog("여긴... 어디지...?", () => {
                    cutSceneIntro.HideCutScene();
                    SetCutSceneState(CutSceneState.Intro_Step3);
                });
                break;
            case CutSceneState.Intro_Step3:
                // 사람 이동 애니메이션 재생. 붕어빵 식탁에 두고 사라짐
                Debug.Log("Intro_Step3! "); 

                break;
            case CutSceneState.Intro_Step4:
                // "이때다! 빨리 나가야겠어!" 다이얼로그 노출

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
    /// 컷씬 시작!
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
