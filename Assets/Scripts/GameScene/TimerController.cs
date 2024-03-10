using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 1. StartTimer() : Ÿ�̸� ����
/// 2. totalPassedTime > 2min : GameManager���� ����. 
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

        //h = ((int)totalPassedTime / 3600);    �ð������� ��� ����. 
        m = ((int)totalPassedTime / 60 % 60);
        s = ((int)totalPassedTime % 60);
        txtTimer.text = $"{m,2:D2} : {s,02:D2}";
    }

    /// <summary>
    /// Timer �ʱ�ȭ ( Start() ���� ȣ��˴ϴ� ) 
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
    /// Ÿ�̸� ���� ( GameManger���� ȣ���մϴ� )
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
