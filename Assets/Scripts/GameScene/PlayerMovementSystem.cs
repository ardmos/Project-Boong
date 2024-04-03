using System.Collections;
using UnityEngine;

public class PlayerMovementSystem : MonoBehaviour
{
    private float moveSpeed;
    private Coroutine moveCoroutine;
    private GameInput gameInput;
    private PlayerStaminaSystem playerStaminaSystem;
    private PlayerAnimationController playerAnimationController;

    private void Awake()
    {
        moveSpeed = Player.DEFAULT_MOVESPEED;

        playerStaminaSystem = GetComponent<PlayerStaminaSystem>();
        gameInput = GetComponent<GameInput>();
        playerAnimationController = GetComponent<PlayerAnimationController>();
    }

    private void OnEnable()
    {
        // Add Callbacks 
        gameInput.OnMoveStarted += GameInput_OnMoveStarted;
        gameInput.OnMoveEnded += GameInput_OnMoveEnded;
    }

    private void OnDisable()
    {
        // Unregister Callbacks
        gameInput.OnMoveStarted -= GameInput_OnMoveStarted;
        gameInput.OnMoveEnded -= GameInput_OnMoveEnded;
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

    private void GameInput_OnMoveEnded(object sender, System.EventArgs e)
    {
        // �ð��� ������ ���� �޽� state���� ����
        StartCoroutine(ChangePlayerStateToRestingOverTime());
    }

    private void GameInput_OnMoveStarted(object sender, System.EventArgs e)
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        // ���¹̳� ������ �̵����� ����
        if (playerStaminaSystem.GetStamina() < Player.DEFAULT_STAMINA_CONSUMPTION) return;

        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, inputVector.y, transform.position.z);
        float moveDistance = moveSpeed * Time.deltaTime;

        // �̵��� ��ġ ���
        Vector3 newPosition = transform.position + moveDir * moveDistance;

        // �̵��� ��ġ�� Raycast ����
        int playerLayer = 9;
        int layerMask = ~(1 << playerLayer);
        RaycastHit2D hit = Physics2D.Raycast(transform.position + moveDir, moveDir, moveDistance, layerMask);

        // �̵��� ��ġ�� ���̸� �̵����� ����
        if (hit.collider != null && hit.collider.CompareTag("Wall"))
        {
            Debug.Log($"{hit.collider.tag}");
            return;
        }

        // �̵�
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = StartCoroutine(MoveSmoothly(newPosition));
        // �̵� �ִϸ��̼� ����
        playerAnimationController.StartJumpAnimation();
        // �̵��ø��� ���¹̳� ���� 
        Player.Instance.ReduceStamina();
        // Player state ����
        Player.Instance.SetPlayerState(PlayerBehaviourState.Moving);
    }

    private IEnumerator ChangePlayerStateToRestingOverTime()
    {
        // �⺻ Player state 
        Player.Instance.SetPlayerState(PlayerBehaviourState.Idle);
        yield return new WaitForSeconds(Player.DEFAULT_RESTING_TIME_REQUIRED);
        // �޽� ����
        Player.Instance.SetPlayerState(PlayerBehaviourState.Resting);
    }

    private IEnumerator MoveSmoothly(Vector3 tartgetPosition)
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = tartgetPosition;
        float elapsedTime = 0f;

        yield return new WaitForSeconds(1/6f);

        while (elapsedTime < 1f)
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapsedTime);
            elapsedTime += Time.deltaTime * 2f;
            yield return null; // ���� �����ӱ��� ���
        }

        transform.position = endPos; // �̵� �Ϸ� �� ��ġ ����
    }
}
