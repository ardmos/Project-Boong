using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    private PlayerControls playerControls;

    public event EventHandler OnDownClicked;
    public event EventHandler OnUpClicked;
    public event EventHandler OnLeftClicked;
    public event EventHandler OnRightClicked;

    private void Awake()
    {
        playerControls = new PlayerControls();
        playerControls.GamePlayGamePad.Enable();   
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerControls.GamePlayGamePad.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        return inputVector;
    }
}
