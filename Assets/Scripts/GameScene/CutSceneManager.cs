using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CutSceneState
{
    Intro_Step1,
    Intro_Step2,
    Intro_Step3,
    Intro_Step4,
    Intro_Step5,
    Timeout_Step1,
    Timeout_Step2,
    Timeout_Step3,
    Timeout_Step4,
    CaughtByPuppy,
    Win_Step1,
    Win_Step2
}

public class CutSceneManager : MonoBehaviour
{
    public static CutSceneManager Instance { get; private set; }

    public HumanController humanController;
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
        
    }

    private void OnGameOverTimeout(object sender, System.EventArgs e)
    {
        SetCutSceneState(CutSceneState.Timeout_Step1);
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
                DialogManager.Instance.ShowDialog("여긴... 어디지...?", () =>
                {
                    cutSceneIntro.HideCutScene();
                    StartCoroutine(WaitAndStartIntroStep3());
                });
                break;
            case CutSceneState.Intro_Step3:
                // 사람 이동 애니메이션 재생. 걸어와서 붕어빵을 테이블에 올려둠
                Debug.Log("Intro_Step3!");
                humanController.Show();
                humanController.MoveToMovePointsIntroStep3(() =>
                {
                    StartCoroutine(WaitAndStartIntroStep4());
                });
                break;
            case CutSceneState.Intro_Step4:
                // 사람 이동 애니메이션 재생. 현관문으로 걸어서 나감
                Debug.Log("Intro_Step4!");
                humanController.MoveToMovePointsIntroStep4(() =>
                {
                    StartCoroutine(WaitAndStartIntroStep5());
                });
                break;
            case CutSceneState.Intro_Step5:
                // "이때다! 빨리 나가야겠어!" 다이얼로그 노출
                Debug.Log("Intro_Step5!");
                DialogManager.Instance.ShowDialog("이때다! 빨리 나가야겠어!", () =>
                {
                    GameManager.Instance.SetGameState(GameState.Phase_1);
                });
                break;
            case CutSceneState.Timeout_Step1:
                // 플레이어 이동 정지
                Player.Instance.DisableMovement();
                StartCoroutine(WaitAndStartTimeoutStep2());
                break;
            case CutSceneState.Timeout_Step2:
                // "앗! 벌써 돌아오다니!!" 다이얼로그 노출 및 현관에 사람 돌아옴
                DialogManager.Instance.ShowDialog("앗! 벌써 돌아오다니!!", () =>
                {
                    StartCoroutine(WaitAndStartTimeoutStep3());
                });
                break;
            case CutSceneState.Timeout_Step3:
                // 사람 약간 이동. 
                humanController.MoveToMovePointsGameOver(() =>
                {
                    StartCoroutine(WaitAndStartTimeoutStep4());
                });
                break;
            case CutSceneState.Timeout_Step4:
                // 컷씬 재생, 컷씬UI 버튼 활성화
                cutSceneTimeout.ShowCutScene();
                cutSceneTimeout.ActivateButtons();
                break;
            case CutSceneState.CaughtByPuppy:
                cutSceneCaughtByPuppy.ShowCutScene();
                cutSceneCaughtByPuppy.ActivateButtons();
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
        yield return new WaitForSeconds(2f);
        SetCutSceneState(CutSceneState.Intro_Step2);
    }

    private IEnumerator WaitAndStartIntroStep3()
    {
        yield return new WaitForSeconds(1f);
        SetCutSceneState(CutSceneState.Intro_Step3);
    }

    private IEnumerator WaitAndStartIntroStep4()
    {
        yield return new WaitForSeconds(1f);
        // 테이블에 붕어빵(Player) 생성
        Player.Instance.PlayerStart();
        SetCutSceneState(CutSceneState.Intro_Step4);
    }

    private IEnumerator WaitAndStartIntroStep5()
    {
        yield return new WaitForSeconds(1f);
        humanController.Hide();
        SetCutSceneState(CutSceneState.Intro_Step5);
    }

    private IEnumerator WaitAndStartTimeoutStep2()
    {
        yield return new WaitForSeconds(1f);
        SetCutSceneState(CutSceneState.Timeout_Step2);
    }
    private IEnumerator WaitAndStartTimeoutStep3()
    {
        yield return new WaitForSeconds(1f);
        humanController.Show();
        yield return new WaitForSeconds(1f);
        SetCutSceneState(CutSceneState.Timeout_Step3);
    }
    private IEnumerator WaitAndStartTimeoutStep4()
    {
        yield return new WaitForSeconds(1f);
        SetCutSceneState(CutSceneState.Timeout_Step4);
    }
}
