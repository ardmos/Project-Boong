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
        moveSpeed = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }

    protected void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, inputVector.y, transform.position.z);
        float moveDistance = moveSpeed * Time.deltaTime;
        transform.position += moveDir * moveDistance;


        //isWalking = moveDir != Vector3.zero;
        // 진행방향 바라볼 때
        //Rotate(moveDir);
        // 마우스 커서 방향 바라볼 때
        //Vector3 mouseDir = GetMouseDir();
        //Rotate(mouseDir);
    }
}
