using System;
using System.Collections;
using UnityEngine;

public struct PlayerData
{
    public float moveSpeed;
    public float stamina;

    public PlayerData(float moveSpeed, float stamina)
    {
        this.moveSpeed = moveSpeed;
        this.stamina = stamina;
    }
}

public enum PlayerState
{
    Idle,
    Moving,
    Resting
}

public class Player : MonoBehaviour
{
    public const float DEFAULT_STAMINA_MAX = 100f;
    public const float DEFAULT_MOVESPEED = 50f;
    public const float DEFAULT_STAMINA_CONSUMPTION = 5f;
    public const float DEFAULT_STAMINA_RECOVERY_AMOUNT = 20f;
    public const float DEFAULT_STAMINA_RECOVERY_INTERVAL = 1f;
    public const float DEFAULT_RESTING_TIME_REQUIRED = 1f;
    
    public static Player Instance { get; private set; }

    public event EventHandler OnExitPointReached;
    public event EventHandler OnCaughtByPuppy;
    public StaminaUIController staminaUIController;
    public PlayerState playerState;

    private PlayerData playerData;
    private Coroutine staminaRecoveryCoroutine;

    private void Awake()
    {
        Debug.Log("Player Awake()");
        Instance = this;
        // Init Player Data
        playerData = new PlayerData(DEFAULT_MOVESPEED, DEFAULT_STAMINA_MAX);
    }

    private void Start()
    {
        SetPlayerState(PlayerState.Idle);
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Exit Point"))
        {
            OnExitPointReached.Invoke(this, new EventArgs());
        }

        if (collision.CompareTag("Puppy"))
        {
            OnCaughtByPuppy.Invoke(this, new EventArgs());
        }

        if (collision.CompareTag("Door"))
        {
            OpenDoor(collision.GetComponent<DoorController>());
        }
    }

    private void PlayerStateMachine()
    {
        switch (playerState)
        {
            case PlayerState.Idle:
                break;
            case PlayerState.Moving:
                // 스태미너 회복 중지
                StopStaminaRecoveryCoroutine();
                break;
            case PlayerState.Resting:
                // 스태미너 회복 시작
                StartStaminaRecoveryCoroutine();
                break;
        }
    }

    private void OpenDoor(DoorController doorController)
    {
        doorController.Open();
    }

    public float GetMoveSpeed()
    {
        return playerData.moveSpeed;
    }
    public float GetStamina()
    {
        return playerData.stamina;
    }
    public void ReduceStamina()
    {
        if (playerData.stamina < DEFAULT_STAMINA_CONSUMPTION) playerData.stamina = 0f;
        else playerData.stamina -= DEFAULT_STAMINA_CONSUMPTION;

        staminaUIController.SetUI(GetStamina());
    }
    private void RecoverStamina()
    {
        if (playerData.stamina + DEFAULT_STAMINA_RECOVERY_AMOUNT > DEFAULT_STAMINA_MAX)
        {
            playerData.stamina = DEFAULT_STAMINA_MAX;
            // 스태미너 회복 중지
            StopStaminaRecoveryCoroutine();
        }
        else playerData.stamina += DEFAULT_STAMINA_RECOVERY_AMOUNT;

        staminaUIController.SetUI(GetStamina());
    }
    public void SetPlayerState(PlayerState playerState)
    {
        this.playerState = playerState;

        PlayerStateMachine();
    }

    private IEnumerator StaminaRecoveryTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(DEFAULT_STAMINA_RECOVERY_INTERVAL);
            RecoverStamina();
        }
    }

    private void StartStaminaRecoveryCoroutine()
    {
        if (staminaRecoveryCoroutine != null) return;

        staminaRecoveryCoroutine = StartCoroutine(StaminaRecoveryTimer());
    }

    private void StopStaminaRecoveryCoroutine()
    {
        if (staminaRecoveryCoroutine == null) return;

        StopCoroutine(staminaRecoveryCoroutine);
        staminaRecoveryCoroutine = null;
    }
}
