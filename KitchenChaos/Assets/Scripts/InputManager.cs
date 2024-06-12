using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    PlayerInputActions playerInputActions;
    private void Awake()
    {
        PlayerInputActions playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
    }

    public Vector2 GetMovementVector()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();       

        inputVector = inputVector.normalized;   //This makes it so diagonal isn't (1,1) and so faster. It calcs pythagoaras and means speed same even if diagonal. magnitude always 1.

        return inputVector;

    }
}
