using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI keyMoveUpText;
    [SerializeField] TextMeshProUGUI keyMoveDownText;
    [SerializeField] TextMeshProUGUI keyMoveLeftText;
    [SerializeField] TextMeshProUGUI keyMoveRightText;
    [SerializeField] TextMeshProUGUI keyInteractText;
    [SerializeField] TextMeshProUGUI keyInteractAlternateInteractText;
    [SerializeField] TextMeshProUGUI keyPauseText;
    [SerializeField] TextMeshProUGUI keyGamepadInteractText;
    [SerializeField] TextMeshProUGUI keyGamepadAlternateInteractText;
    [SerializeField] TextMeshProUGUI keyGamepadPauseText;

    private void Start()
    {
        InputManager.Instance.OnBindingRebound += InputManager_OnBindingRebound;         //If mappings change, tutorial changes.
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;

        UpdateVisual();

        Show();
    }

    private void KitchenGameManager_OnStateChanged(object sender, System.EventArgs e)     //Hide tutorial when interact pressed!
    {
        if (KitchenGameManager.Instance.IsCountDownToStartActive())
        {
            Hide();
        }
    }

    private void InputManager_OnBindingRebound(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    void UpdateVisual()
    {
        keyMoveUpText.text = InputManager.Instance.GetBindingText(InputManager.Binding.Move_Up);
        keyMoveDownText.text = InputManager.Instance.GetBindingText(InputManager.Binding.Move_Down);
        keyMoveLeftText.text = InputManager.Instance.GetBindingText(InputManager.Binding.Move_Left);
        keyMoveRightText.text = InputManager.Instance.GetBindingText(InputManager.Binding.Move_Right);
        keyInteractText.text = InputManager.Instance.GetBindingText(InputManager.Binding.Interact);
        keyInteractAlternateInteractText.text = InputManager.Instance.GetBindingText(InputManager.Binding.InteractAlternate);
        keyPauseText.text = InputManager.Instance.GetBindingText(InputManager.Binding.Pause);
        keyGamepadInteractText.text = InputManager.Instance.GetBindingText(InputManager.Binding.Gamepad_Interact);
        keyGamepadAlternateInteractText.text = InputManager.Instance.GetBindingText(InputManager.Binding.Gamepad_InteractAlternate);
        keyGamepadPauseText.text = InputManager.Instance.GetBindingText(InputManager.Binding.Gamepad_Pause);
    }

    void Show() => gameObject.SetActive(true);
    void Hide() => gameObject.SetActive(false);

}
