using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{

    public static OptionsUI Instance { get; private set; }


    [SerializeField] Button soundEffectsButton;
    [SerializeField] Button musicButton;
    [SerializeField] Button closeButton;

    [SerializeField] TextMeshProUGUI soundEffectsText;
    [SerializeField] TextMeshProUGUI musicText;

    [SerializeField] TextMeshProUGUI moveUpText;
    [SerializeField] TextMeshProUGUI moveDownText;
    [SerializeField] TextMeshProUGUI moveLeftText;
    [SerializeField] TextMeshProUGUI moveRightText;
    [SerializeField] TextMeshProUGUI interactText;
    [SerializeField] TextMeshProUGUI interactAlternateText;
    [SerializeField] TextMeshProUGUI pauseText;
    [SerializeField] TextMeshProUGUI gamepadInteractText;
    [SerializeField] TextMeshProUGUI gamepadInteractAlternateText;
    [SerializeField] TextMeshProUGUI gamepadPauseText;

    [SerializeField] Button moveUpButton;
    [SerializeField] Button moveDownButton;
    [SerializeField] Button moveRightButton;
    [SerializeField] Button moveLeftButton;
    [SerializeField] Button interactButton;
    [SerializeField] Button interactAlternateButton;
    [SerializeField] Button pauseButton;
    [SerializeField] Button gamepadInteractButton;
    [SerializeField] Button gamepadInteractAlternateButton;
    [SerializeField] Button gamepadPauseButton;

    [SerializeField] Transform pressToRebindKeyTransform;

    Action onCloseButtonAction;


    private void Awake()
    {
        Instance = this;


        soundEffectsButton.onClick.AddListener(() =>             //Lambda expression adds on click event, same as if did in editor.
        {
            SoundManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        musicButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        closeButton.onClick.AddListener(() =>
        {
            Hide();
            onCloseButtonAction();        //Shows pause menu again.
        });

        moveUpButton.onClick.AddListener(() => { RebindBinding(InputManager.Binding.Move_Up); });
        moveDownButton.onClick.AddListener(() => { RebindBinding(InputManager.Binding.Move_Down); });
        moveLeftButton.onClick.AddListener(() => { RebindBinding(InputManager.Binding.Move_Left); });
        moveRightButton.onClick.AddListener(() => { RebindBinding(InputManager.Binding.Move_Right); });
        interactButton.onClick.AddListener(() => { RebindBinding(InputManager.Binding.Interact); });
        interactAlternateButton.onClick.AddListener(() => { RebindBinding(InputManager.Binding.InteractAlternate); });
        pauseButton.onClick.AddListener(() => { RebindBinding(InputManager.Binding.Pause); });
        gamepadInteractButton.onClick.AddListener(() => { RebindBinding(InputManager.Binding.Gamepad_Interact); });
        gamepadInteractAlternateButton.onClick.AddListener(() => { RebindBinding(InputManager.Binding.Gamepad_InteractAlternate); });
        gamepadPauseButton.onClick.AddListener(() => { RebindBinding(InputManager.Binding.Gamepad_Pause); });
    }

    private void Start()
    {
        KitchenGameManager.Instance.OnGameUnpaused += KitchenGameManager_OnGameUnpaused;

        UpdateVisual();

        Hide();                       //Hide visuals by default.
        HidePressToRebindKey();
    }

    private void KitchenGameManager_OnGameUnpaused(object sender, System.EventArgs e)
    {
        Hide();
    }

    void UpdateVisual()
    {
        soundEffectsText.text = "Sound Effects: " + Mathf.Round(SoundManager.Instance.Volume * 10f);
        musicText.text = "Music: " + Mathf.Round(MusicManager.Instance.Volume * 10f);

        moveUpText.text = InputManager.Instance.GetBindingText(InputManager.Binding.Move_Up);
        moveDownText.text = InputManager.Instance.GetBindingText(InputManager.Binding.Move_Down);
        moveLeftText.text = InputManager.Instance.GetBindingText(InputManager.Binding.Move_Left);
        moveRightText.text = InputManager.Instance.GetBindingText(InputManager.Binding.Move_Right);
        interactText.text = InputManager.Instance.GetBindingText(InputManager.Binding.Interact);
        interactAlternateText.text = InputManager.Instance.GetBindingText(InputManager.Binding.InteractAlternate);
        pauseText.text = InputManager.Instance.GetBindingText(InputManager.Binding.Pause);
        gamepadInteractText.text = InputManager.Instance.GetBindingText(InputManager.Binding.Gamepad_Interact);
        gamepadInteractAlternateText.text = InputManager.Instance.GetBindingText(InputManager.Binding.Gamepad_InteractAlternate);
        gamepadPauseText.text = InputManager.Instance.GetBindingText(InputManager.Binding.Gamepad_Pause);

    }

    public void Show(Action onCloseButtonAction)
    {
        this.onCloseButtonAction = onCloseButtonAction;         //Storing delegate to show pause menu when this is deactivated.


        gameObject.SetActive(true);

        soundEffectsButton.Select();         //For controller.
    }


    public void Hide() => gameObject.SetActive(false);

    public void ShowPressToRebindKey() => pressToRebindKeyTransform.gameObject.SetActive(true);
    public void HidePressToRebindKey()
    {
        pressToRebindKeyTransform.gameObject.SetActive(false);      
        UpdateVisual();             //In case they rebound.
    }


    void RebindBinding(InputManager.Binding binding)
    {
        ShowPressToRebindKey();
        InputManager.Instance.RebindBinding(binding, HidePressToRebindKey);      //Second param is a delegate so input manager script can run that function.
    }


}
