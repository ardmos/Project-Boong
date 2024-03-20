using System;
using UnityEngine;

public enum GameState
{
    Intro,
    Phase_1,
    Phase_2,
    GameOver_Timeout,
    GameOver_ByPuppy,
    Win
}

public class GameManager : MonoBehaviour
{
    public const float PHASE1_DURATION = 1f;
    public const float PHASE2_DURATION = PHASE1_DURATION + 2f;

    public static GameManager Instance { get; private set; }

    public event EventHandler OnPhase_1;
    public event EventHandler OnGameOverTimeout;
    public event EventHandler OnGameOverByPuppy;
    public event EventHandler OnGameWin;

    public TimerController timerController;
    public GameState gameState;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    private void Start()
    {
        Debug.Log($"Game Manager Start");

        // Add Callbacks
        timerController.Phase1TimeEnded += TimerController_Phase1TimeEnded;
        timerController.Phase2TimeEnded += TimerController_Phase2TimeEnded;
        Player.Instance.OnExitPointReached += Player_OnExitPointReached;
        Player.Instance.OnCaughtByPuppy += Player_OnCaughtByPuppy;

        SetGameState(GameState.Intro);
    }

    private void OnDisable()
    {
        // Unregister Callbacks
        timerController.Phase1TimeEnded -= TimerController_Phase1TimeEnded;
        timerController.Phase2TimeEnded -= TimerController_Phase2TimeEnded;

        if (Player.Instance == null)
        {
            Debug.Log("OnDisable(): Player.Instance is null");
            return;
        }
        Player.Instance.OnExitPointReached -= Player_OnExitPointReached;
        Player.Instance.OnCaughtByPuppy -= Player_OnCaughtByPuppy;
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    private void Player_OnCaughtByPuppy(object sender, System.EventArgs e)
    {
        SetGameState(GameState.GameOver_ByPuppy);
    }

    private void Player_OnExitPointReached(object sender, System.EventArgs e)
    {
        SetGameState(GameState.Win);
    }

    private void TimerController_Phase2TimeEnded(object sender, System.EventArgs e)
    {
        SetGameState(GameState.GameOver_Timeout);
    }

    private void TimerController_Phase1TimeEnded(object sender, System.EventArgs e)
    {
        SetGameState(GameState.Phase_2);
    }

    private void GameStateMachine()
    {
        switch (gameState)
        {
            case GameState.Intro:
                StartCutScene();
                break;
            case GameState.Phase_1:
                StartPhase1();
                break;
            case GameState.Phase_2:
                StartPhase2();
                break;
            case GameState.GameOver_Timeout:
                GameOverTimeout();
                break;
            case GameState.GameOver_ByPuppy:
                GameOverByPuppy();
                break;
            case GameState.Win:
                Win();
                break;
        }
    }

    public void SetGameState(GameState gameState)
    {
        this.gameState = gameState;
        GameStateMachine();
    }

    /// <summary>
    /// Intro State 시작. 시작 컷신이 재생되어야 합니다. 
    /// 재생이 완료되면 다음 State로 넘겨줍니다.
    /// </summary>
    private void StartCutScene()
    {
        Debug.Log("ShowCutScene!");
        CutSceneManager.Instance.StartCutScene();   
    }

    /// <summary>
    /// 1페이즈 시작
    /// </summary>
    private void StartPhase1()
    {
        Debug.Log("Phase 1!");
        timerController.StartTimer();
    }

    /// <summary>
    /// 2페이즈 시작. 
    /// </summary>
    private void StartPhase2()
    {
        Debug.Log("Phase 2!");
    }

    private void GameOverTimeout()
    {
        Debug.Log("Game Over! Timeout!");
        timerController.StopTimer();
        OnGameOverTimeout.Invoke(this, EventArgs.Empty);
    }

    private void GameOverByPuppy()
    {
        Debug.Log("Game Over! By Puppy!");
        timerController.StopTimer();
        OnGameOverByPuppy.Invoke(this, EventArgs.Empty);
    }

    private void Win()
    {
        Debug.Log("Win !");
        timerController.StopTimer();
        OnGameWin.Invoke(this, EventArgs.Empty);
    }
}
