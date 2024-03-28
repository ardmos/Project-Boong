using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float moveSpeed;
    private Coroutine moveCoroutine;

    public GameInput gameInput;

    private void Start()
    {
        // Add Callbacks 
        gameInput.OnMoveStarted += GameInput_OnMoveStarted;
        gameInput.OnMoveEnded += GameInput_OnMoveEnded;

        moveSpeed = Player.Instance.GetMoveSpeed();
    }

    private void OnDisable()
    {
        // Unregister Callbacks
        gameInput.OnMoveStarted -= GameInput_OnMoveStarted;
        gameInput.OnMoveEnded -= GameInput_OnMoveEnded;
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
        // ���¹̳� �ܿ��� ������ �̵����� ����
        if (Player.Instance.GetStamina() < Player.DEFAULT_STAMINA_CONSUMPTION) return;

        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, inputVector.y, transform.position.z);
        float moveDistance = moveSpeed * Time.deltaTime;

        // �̵��� ��ġ ���
        Vector3 newPosition = transform.position + moveDir * moveDistance;

        // �浹 ���� Ȯ��
        int playerLayer = 9;
        int layerMask = ~(1 << playerLayer);
        RaycastHit2D hit = Physics2D.Raycast(transform.position + moveDir, moveDir, moveDistance, layerMask);
        
        if (hit.collider != null && hit.collider.CompareTag("Wall"))
        {
            Debug.Log($"{hit.collider.tag}");
            // ���� ������ �̵����� ����
            return;
        }

        // �̵�
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = StartCoroutine(MoveSmoothly(newPosition));
        // �̵� �ִϸ��̼� ����
        GetComponentInChildren<PlayerAnimationController>().StartJumpAnimation();
        // �̵��ø��� ���¹̳� ���� 
        Player.Instance.ReduceStamina();
        // Player state ����
        Player.Instance.SetPlayerState(PlayerState.Moving);

        // ������� �ٶ� ��
        //Rotate(moveDir);
        // ���콺 Ŀ�� ���� �ٶ� ��
        //Vector3 mouseDir = GetMouseDir();
        //Rotate(mouseDir);
    }

    private IEnumerator ChangePlayerStateToRestingOverTime()
    {
        // �⺻ Player state 
        Player.Instance.SetPlayerState(PlayerState.Idle);
        yield return new WaitForSeconds(Player.DEFAULT_RESTING_TIME_REQUIRED);
        // �޽� ����
        Player.Instance.SetPlayerState(PlayerState.Resting);
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
