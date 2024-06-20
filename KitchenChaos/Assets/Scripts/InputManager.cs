using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }             //Singleton obviously.

    public event EventHandler OnInteractPressed;
    public event EventHandler OnInteractAlternatePressed;
    public event EventHandler OnPausePressed;

    public enum Binding { Move_Up, Move_Down, Move_Left, Move_Right, Interact, InteractAlternate, Pause, Gamepad_Interact, Gamepad_InteractAlternate, Gamepad_Pause }      //Input key bindings.

    PlayerInputActions playerInputActions;
    private void Awake()
    {
        Instance = this;

        playerInputActions = new PlayerInputActions();

        if (PlayerPrefs.HasKey("InputBindings"))                         //Loads from previous session if we have saved bindings.
        {
            playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString("InputBindings"));
        }

        playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed += Interact_performed;      //Because interact is a button, it is an event, not continuous checks, way more efficient. Subscribe to event.
        playerInputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
        playerInputActions.Player.Pause.performed += Pause_performed;


    }

    private void OnDestroy()
    {
        playerInputActions.Player.Interact.performed -= Interact_performed;                          //Unsubscribe. So will not trigger once destroyed (like on scene change).
        playerInputActions.Player.InteractAlternate.performed -= InteractAlternate_performed;
        playerInputActions.Player.Pause.performed -= Pause_performed;

        playerInputActions.Dispose();             //Cleans up object from memory. 
    }

    private void Pause_performed(InputAction.CallbackContext obj)   //Fires when pause pressed.
    {
        OnPausePressed?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(InputAction.CallbackContext obj)
    {
        OnInteractPressed?.Invoke(this, EventArgs.Empty);      //Fire event when interact pressed.   Invoke ? means only fire if there are listeners.
    }

    private void InteractAlternate_performed(InputAction.CallbackContext obj)
    {
        OnInteractAlternatePressed?.Invoke(this, EventArgs.Empty);      //Fire event when interact pressed.   Invoke ? means only fire if there are listeners.
    }

    public Vector2 GetMovementVector()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;   //This makes it so diagonal isn't (1,1) and so faster. It calcs pythagoaras and means speed same even if diagonal. magnitude always 1.

        return inputVector;

    }


    public string GetBindingText(Binding binding)           //Returns the binding of a selected key.
    {
        switch (binding)
        {
            default:
            case Binding.Move_Up:
                return playerInputActions.Player.Move.bindings[1].ToDisplayString();          //Return array of bindings we set up. WASD is each one in the array. See Codemonkey overcooked. 9hr 30.
            case Binding.Move_Down:
                return playerInputActions.Player.Move.bindings[2].ToDisplayString();          
            case Binding.Move_Left:
                return playerInputActions.Player.Move.bindings[3].ToDisplayString();          
            case Binding.Move_Right:
                return playerInputActions.Player.Move.bindings[4].ToDisplayString();          
            case Binding.Interact:
                return playerInputActions.Player.Interact.bindings[0].ToDisplayString();          //Return array of bindings we set up in new input system. We always did index 0.
            case Binding.InteractAlternate:
                return playerInputActions.Player.InteractAlternate.bindings[0].ToDisplayString();          //Return array of bindings we set up in new input system. We always did index 0.
            case Binding.Pause:
                return playerInputActions.Player.Pause.bindings[0].ToDisplayString();          //Return array of bindings we set up in new input system. We always did index 0.
            case Binding.Gamepad_Interact:
                return playerInputActions.Player.Interact.bindings[1].ToDisplayString();          //Return array of bindings we set up in new input system. We always did index 0.
            case Binding.Gamepad_InteractAlternate:
                return playerInputActions.Player.InteractAlternate.bindings[1].ToDisplayString();          //Return array of bindings we set up in new input system. We always did index 0.
            case Binding.Gamepad_Pause:
                return playerInputActions.Player.Pause.bindings[1].ToDisplayString();          //Return array of bindings we set up in new input system. We always did index 0.

        }
    }

    public void RebindBinding(Binding binding, Action onInputRebound)       //Remappings. Action is a simple delegate, no return and no params.
    {
        playerInputActions.Player.Disable();

        InputAction inputAction;           //So we can remapp dynamically without manually copying pasting all the code  below just with different inputs and indexes.
        int bindingIndex;

        switch (binding)
        {
            default:
            case Binding.Move_Up:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 1;
                break;
            case Binding.Move_Down:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 2;
                break;
            case Binding.Move_Left:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 3;
                break;
            case Binding.Move_Right:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 4;
                break;
            case Binding.Interact:
                inputAction = playerInputActions.Player.Interact;
                bindingIndex = 0;
                break;
            case Binding.InteractAlternate:
                inputAction = playerInputActions.Player.InteractAlternate;
                bindingIndex = 0;
                break;
            case Binding.Pause:
                inputAction = playerInputActions.Player.Pause;
                bindingIndex = 0;
                break;
            case Binding.Gamepad_Interact:
                inputAction = playerInputActions.Player.Interact;
                bindingIndex = 1;
                break;
            case Binding.Gamepad_InteractAlternate:
                inputAction = playerInputActions.Player.InteractAlternate;
                bindingIndex = 1;
                break;
            case Binding.Gamepad_Pause:
                inputAction = playerInputActions.Player.Pause;
                bindingIndex = 1;
                break;
        }

        inputAction.PerformInteractiveRebinding(bindingIndex).OnComplete(callback =>
        {
            callback.Dispose();                              //memory cleanup.
            playerInputActions.Player.Enable();

            onInputRebound();         //Run delegate.

            PlayerPrefs.SetString("InputBindings", playerInputActions.SaveBindingOverridesAsJson());        //Easy function to save input remappings between sessions.
            PlayerPrefs.Save();
        })
            .Start();          //Start starts rebinding process.
    }

}



