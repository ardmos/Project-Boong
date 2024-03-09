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
        isOnPhase1TimeEndedInvoked = false;
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
}
