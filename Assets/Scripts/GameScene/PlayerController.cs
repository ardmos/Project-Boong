using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
        // 시간이 지남에 따른 휴식 state로의 변경
        StartCoroutine(ChangePlayerStateToRestingOverTime());
    }

    private void GameInput_OnMoveStarted(object sender, System.EventArgs e)
    {
        HandleMovement();
    }

    protected void HandleMovement()
    {
        // 스태미너 잔여량 부족시 이동하지 않음
        if (Player.Instance.GetStamina() < Player.DEFAULT_STAMINA_CONSUMPTION) return;

        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, inputVector.y, transform.position.z);
        float moveDistance = moveSpeed * Time.deltaTime;
        transform.position += moveDir * moveDistance;

        // 이동시마다 스태미너 감소 
        Player.Instance.ReduceStamina();
        // Player state 변경
        Player.Instance.SetPlayerState(PlayerState.Moving);

        //isWalking = moveDir != Vector3.zero;
        // 진행방향 바라볼 때
        //Rotate(moveDir);
        // 마우스 커서 방향 바라볼 때
        //Vector3 mouseDir = GetMouseDir();
        //Rotate(mouseDir);
    }

    private IEnumerator ChangePlayerStateToRestingOverTime()
    {
        // 기본 Player state 
        Player.Instance.SetPlayerState(PlayerState.Idle);
        yield return new WaitForSeconds(Player.DEFAULT_RESTING_TIME_REQUIRED);
        // 휴식 상태
        Player.Instance.SetPlayerState(PlayerState.Resting);
    }
}
