using System;
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
    public const float DEFAULT_MOVESPEED = 100f;
    public const float DEFAULT_STAMINA_CONSUMPTION = 5f;
    public const float DEFAULT_STAMINA_RECOVERY_AMOUNT = 20f;
    public const float DEFAULT_STAMINA_RECOVERY_INTERVAL = 1f;
    public const float DEFAULT_RESTING_TIME_REQUIRED = 1f;
    
    public static Player Instance { get; private set; }

    public event EventHandler OnExitPointReached;
    public event EventHandler OnCaughtByPuppy;
    
    public Transform playerStartPoint; 

    private PlayerData playerData;
    private PlayerState playerState;
    private PlayerMovementSystem playerMovementSystem;
    private PlayerStaminaSystem playerStaminaSystem;
    private PlayerEmotionSystem playerEmotionSystem;
    private GameInput gameInput;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        Instance = this;

        playerMovementSystem = GetComponent<PlayerMovementSystem>();
        playerStaminaSystem = GetComponent<PlayerStaminaSystem>();
        playerEmotionSystem = GetComponent<PlayerEmotionSystem>();
        gameInput = GetComponent<GameInput>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        PuppyAI.Instance.OnChasingStart += Puppy_OnChasingStart;
        GameManager.Instance.OnGameOverTimeout += OnGameOverTimeout;

        // Init Player Data
        playerData = new PlayerData(playerMovementSystem.GetMoveSpeed(), playerStaminaSystem.GetStamina());
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
            DoorNames doorName;
            if (Enum.TryParse(collision.name, out doorName))
            {
                DoorManager.Instance.OpenDoor(doorName);
            }
            else
            {
                Debug.LogWarning("Door name does not match any enum value.");
            }
        }
    }

    private void OnDisable()
    {
        if(PuppyAI.Instance == null || GameManager.Instance == null) return;

        PuppyAI.Instance.OnChasingStart -= Puppy_OnChasingStart;
        GameManager.Instance.OnGameOverTimeout -= OnGameOverTimeout;
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    public void ActivatePlayer()
    {
        spriteRenderer.enabled = true;
    }

    public void ResetPlayer()
    {
        spriteRenderer.enabled = false;
        transform.position = playerStartPoint.position;
        SetPlayerState(PlayerState.Idle);
    }

    public PlayerData GetPlayerData()
    {
        return playerData;
    }

    public void UpdatePlayerStamina(float stamina)
    {
        playerData.stamina = stamina;
    }

    public void ReduceStamina()
    {
        playerStaminaSystem.ReduceStamina();
    }

    public void SetPlayerState(PlayerState playerState)
    {
        this.playerState = playerState;

        PlayerStateMachine();
    }

    public void DisableControl()
    {
        gameInput.SetControllable(false);
    }

    private void Puppy_OnChasingStart(object sender, EventArgs e)
    {
        playerEmotionSystem.CreateEmotionBubble(PlayerEmotions.Shock);
    }

    private void OnGameOverTimeout(object sender, EventArgs e)
    {
        playerEmotionSystem.CreateEmotionBubble(PlayerEmotions.GameOver);
    }

    private void PlayerStateMachine()
    {
        switch (playerState)
        {
            case PlayerState.Idle:
                break;
            case PlayerState.Moving:
                // 스태미너 회복 중지
                playerStaminaSystem.StopStaminaRecovery();
                break;
            case PlayerState.Resting:
                // 스태미너 회복 시작
                playerStaminaSystem.StartStaminaRecovery();
                break;
        }
    }
}
