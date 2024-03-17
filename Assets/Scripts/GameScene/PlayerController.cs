using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameInput gameInput;

    private float moveSpeed;

    void Start()
    {
        moveSpeed = Player.Instance.GetMoveSpeed();

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

    private void GameInput_OnMoveEnded(object sender, System.EventArgs e)
    {
        // �ð��� ������ ���� �޽� state���� ����
        StartCoroutine(ChangePlayerStateToRestingOverTime());
    }

    private void GameInput_OnMoveStarted(object sender, System.EventArgs e)
    {
        HandleMovement();
    }

    protected void HandleMovement()
    {
        // ���¹̳� �ܿ��� ������ �̵����� ����
        if (Player.Instance.GetStamina() < Player.DEFAULT_STAMINA_CONSUMPTION) return;

        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, inputVector.y, transform.position.z);
        float moveDistance = moveSpeed * Time.deltaTime;

        // �̵��� ��ġ ���
        Vector3 newPosition = transform.position + moveDir * moveDistance;

        //Debug.Log($"{transform.position}, {moveDir}, {moveDistance}");

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
        transform.position = newPosition;

        // �̵��ø��� ���¹̳� ���� 
        Player.Instance.ReduceStamina();
        // Player state ����
        Player.Instance.SetPlayerState(PlayerState.Moving);

        //isWalking = moveDir != Vector3.zero;
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
}
