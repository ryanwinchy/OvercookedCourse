using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    public event EventHandler OnInteractPressed;

    PlayerInputActions playerInputActions;
    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed += Interact_performed;      //Because interact is a button, it is an event, not continuous checks, way more efficient. Subscribe to event.
    }

    private void Interact_performed(InputAction.CallbackContext obj)
    {
        OnInteractPressed?.Invoke(this, EventArgs.Empty);      //Fire event when interact pressed.   Invoke ? means only fire if there are listeners.
    }

    public Vector2 GetMovementVector()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();       

        inputVector = inputVector.normalized;   //This makes it so diagonal isn't (1,1) and so faster. It calcs pythagoaras and means speed same even if diagonal. magnitude always 1.

        return inputVector;

    }
}



