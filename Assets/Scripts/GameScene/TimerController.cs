using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 1. StartTimer() : 타이머 시작
/// 2. totalPassedTime > 2min : GameManager에게 보고. 
/// </summary>
public class TimerController : MonoBehaviour
{
    [SerializeField] private bool isTimerOn;
    [SerializeField] private bool isPhase1TimeEnded;
    [SerializeField] private bool isPhase2TimeEnded;
    [SerializeField] private TextMeshProUGUI txtTimer;
    [SerializeField] private float totalPassedTime;
    int h, m, s;

    public event EventHandler Phase1TimeEnded;
    public event EventHandler Phase2TimeEnded;

    private void Awake()
    {
        InitTimer();    
    }

    void Update()
    {
        if (!isTimerOn) return;
        if (m >= GameManager.PHASE1_DURATION && !isPhase1TimeEnded)
        {
            Phase1TimeEnded.Invoke(this, EventArgs.Empty);
            isPhase1TimeEnded = true;
        }
        if (m >= GameManager.PHASE2_DURATION && !isPhase2TimeEnded)
        {
            Phase2TimeEnded.Invoke(this, EventArgs.Empty);
            isPhase2TimeEnded = true;
        }

        totalPassedTime += Time.deltaTime;

        //h = ((int)totalPassedTime / 3600);    시간단위는 계산 안함. 
        m = ((int)totalPassedTime / 60 % 60);
        s = ((int)totalPassedTime % 60);
        txtTimer.text = $"{m,2:D2} : {s,02:D2}";
    }

    /// <summary>
    /// Timer 초기화 ( Start() 에서 호출됩니다 ) 
    /// </summary>
    private void InitTimer()
    {
        Debug.Log($"{nameof(InitTimer)}");
        totalPassedTime = 0f;
        isTimerOn = false;
        isPhase1TimeEnded = false;
        txtTimer.text = "00 : 00";
    }

    /// <summary>
    /// 타이머 시작 ( GameManger에서 호출합니다 )
    /// </summary>
    public void StartTimer()
    {
        Debug.Log($"{nameof(StartTimer)}");
        isTimerOn = true;
    }

    public void StopTimer()
    {
        Debug.Log($"{nameof(StopTimer)}");
        isTimerOn = false;
    }
}
