using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public GameInput gameInput;

    private float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 100f;

        // Add Callbacks 
        gameInput.OnMoveStarted += GameInput_OnMoveStarted;
        gameInput.OnMoveEnded += GameInput_OnMoveEnded;
    }

    private void OnDestroy()
    {
        // Unregister Callbacks
        gameInput.OnMoveStarted -= GameInput_OnMoveStarted;
        gameInput.OnMoveEnded -= GameInput_OnMoveEnded;
    }

    private void GameInput_OnMoveEnded(object sender, System.EventArgs e)
    {
        HandleMovement();
    }

    private void GameInput_OnMoveStarted(object sender, System.EventArgs e)
    {
        HandleMovement();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    protected void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, inputVector.y, transform.position.z);
        float moveDistance = moveSpeed * Time.deltaTime;
        transform.position += moveDir * moveDistance;


        //isWalking = moveDir != Vector3.zero;
        // ������� �ٶ� ��
        //Rotate(moveDir);
        // ���콺 Ŀ�� ���� �ٶ� ��
        //Vector3 mouseDir = GetMouseDir();
        //Rotate(mouseDir);
    }
}
