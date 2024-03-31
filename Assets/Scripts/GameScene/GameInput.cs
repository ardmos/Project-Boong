using System;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public event EventHandler OnMoveStarted;
    public event EventHandler OnMoveEnded;

    private PlayerControls playerControls;
    private bool isControllable;

    private void Awake()
    {
        playerControls = new PlayerControls();
        playerControls.GamePlayGamePad.Enable();
    }

    private void OnEnable()
    {
        // Add Callbacks 
        playerControls.GamePlayGamePad.Move.started += Move_started;
        playerControls.GamePlayGamePad.Move.canceled += Move_canceled;
    }

    private void Start()
    {
        SetControllable(true);
    }

    private void OnDisable()
    {
        // Unregister Callbacks
        playerControls.GamePlayGamePad.Move.started -= Move_started;
        playerControls.GamePlayGamePad.Move.canceled -= Move_canceled;
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerControls.GamePlayGamePad.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;

        return inputVector;
    }

    public void SetControllable(bool isControllable)
    {
        this.isControllable = isControllable;
    }

    private void Move_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnMoveEnded.Invoke(this, EventArgs.Empty);
    }

    private void Move_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (!isControllable) return;
        OnMoveStarted.Invoke(this, EventArgs.Empty);
    }
}
