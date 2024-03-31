using System;
using TMPro;
using UnityEngine;

/// <summary>
/// 1. StartTimer() : Ÿ�̸� ����
/// 2. totalPassedTime > 2min : GameManager���� ����. 
/// </summary>
public class TimerController : MonoBehaviour
{
    public event EventHandler Phase1TimeEnded;
    public event EventHandler Phase2TimeEnded;

    [SerializeField] private bool isTimerOn;
    [SerializeField] private bool isPhase1TimeEnded;
    [SerializeField] private bool isPhase2TimeEnded;
    [SerializeField] private TextMeshProUGUI txtTimer;
    [SerializeField] private float totalPassedTime;
    private int h, m, s;

    private void Awake()
    {
        ResetTimer();    
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
    /// Timer �ʱ�ȭ ( Awake() ���� ȣ��˴ϴ� ) 
    /// </summary>
    public void ResetTimer()
    {
        Debug.Log($"{nameof(ResetTimer)}");
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
