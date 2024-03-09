using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    const float PHASE1_END_TIME_MIN = 1f;

    public static GameManager Instance { get; private set; }

    public TimerController timerController;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        timerController.OnPhase1TimeEnded += TimerController_OnPhase1TimeEnded;

        timerController.StartTimer();
    }

    private void TimerController_OnPhase1TimeEnded(object sender, System.EventArgs e)
    {
        StartPhase2();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        timerController.OnPhase1TimeEnded -= TimerController_OnPhase1TimeEnded;
    }

    /// <summary>
    /// 1페이즈 시작
    /// </summary>
    private void StartPhase1()
    {

    }

    /// <summary>
    /// 2페이즈 시작. 
    /// </summary>
    public void StartPhase2()
    {
        Debug.Log($"{nameof(StartPhase2)}");
    }

    /// <summary>
    /// Phase1 종료 시간 반환. 분(Min) 기준
    /// </summary>
    public float GetPhase1EndTime() {
        return PHASE1_END_TIME_MIN;
    }
}
