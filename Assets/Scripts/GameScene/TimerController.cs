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
    [SerializeField] private bool isOnPhase1TimeEndedInvoked;
    [SerializeField] private TextMeshProUGUI txtTimer;
    [SerializeField] private float totalPassedTime;
    int h, m, s;

    public event EventHandler OnPhase1TimeEnded;

    private void Awake()
    {
        InitTimer();    
    }

    void Update()
    {
        if (!isTimerOn) return;
        if (m >= GameManager.Instance.GetPhase1EndTime() && !isOnPhase1TimeEndedInvoked)
        {
            OnPhase1TimeEnded.Invoke(this, EventArgs.Empty);
            isOnPhase1TimeEndedInvoked = true;
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
        isOnPhase1TimeEndedInvoked = false;
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
}
