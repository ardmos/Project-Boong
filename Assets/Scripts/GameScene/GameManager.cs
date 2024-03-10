using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Ready,
    Phase1,
    Phase2,
    GameOver,
    Win
}

public class GameManager : MonoBehaviour
{
    public const float PHASE1_DURATION = 1f;
    public const float PHASE2_DURATION = PHASE1_DURATION + 2f;

    public static GameManager Instance { get; private set; }

    public TimerController timerController;
    public GameState gameState;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        timerController.Phase1TimeEnded += TimerController_Phase1TimeEnded;
        timerController.Phase2TimeEnded += TimerController_Phase2TimeEnded;
        Player.Instance.OnExitPointReached += Player_OnExitPointReached;

        Debug.Log($"{nameof(Start)} Game Manager");
        SetGameState(GameState.Ready);

        // 원래 시작컷씬 재생 후 Phase1 시작 해야하는데 지금은 시작컷씬이 없어서 바로 시작합니다. 
        SetGameState(GameState.Phase1);
    }

    private void Player_OnExitPointReached(object sender, System.EventArgs e)
    {
        SetGameState(GameState.Win);
    }

    private void TimerController_Phase2TimeEnded(object sender, System.EventArgs e)
    {
        SetGameState(GameState.GameOver);
    }

    private void TimerController_Phase1TimeEnded(object sender, System.EventArgs e)
    {
        SetGameState(GameState.Phase2);
    }

    private void OnDestroy()
    {
        timerController.Phase1TimeEnded -= TimerController_Phase1TimeEnded;
    }

    private void GameStateMachine()
    {
        switch (gameState)
        {
            case GameState.Ready:
                break;
            case GameState.Phase1:
                StartPhase1();
                break;
            case GameState.Phase2:
                StartPhase2();
                break;
            case GameState.GameOver:
                GameOver();
                break;
            case GameState.Win:
                Win();
                break;
        }
    }

    private void SetGameState(GameState gameState)
    {
        this.gameState = gameState;
        GameStateMachine();
    }

    /// <summary>
    /// Ready State 시작. 시작 컷신이 재생되어야 합니다. 
    /// 재생이 완료되면 다음 State로 넘겨줍니다.
    /// </summary>
    private void Ready()
    {

    }

    /// <summary>
    /// 1페이즈 시작
    /// </summary>
    private void StartPhase1()
    {
        timerController.StartTimer();
       
    }

    /// <summary>
    /// 2페이즈 시작. 
    /// </summary>
    private void StartPhase2()
    {
       
    }

    private void GameOver()
    {
        Debug.Log("Game Over !");
        timerController.StopTimer();
    }

    private void Win()
    {
        Debug.Log("Win !");
        timerController.StopTimer();
    }
}
