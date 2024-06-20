using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{

    [SerializeField] Button resumeButton;
    [SerializeField] Button mainMenuButton;
    [SerializeField] Button optionsButton;

    private void Awake()
    {

        resumeButton.onClick.AddListener(() =>                 //Lambda function. Same as adding on click event in editor.
        {
            KitchenGameManager.Instance.TogglePauseGame();
        });
        mainMenuButton.onClick.AddListener(() =>                 //Lambda function. Same as adding on click event in editor.
        {
            Loader.Load(Loader.Scene.MainMenuScene);
        });
        optionsButton.onClick.AddListener(() =>                 //Lambda function. Same as adding on click event in editor.
        {
            Hide();                                //Hide pause menu when go to options.
            OptionsUI.Instance.Show(Show);                //Delegates show to options menu, so this can show when options closed.
        });
    }
    private void Start()
    {
        KitchenGameManager.Instance.OnGamePaused += KitchenGameManager_OnGamePaused;
        KitchenGameManager.Instance.OnGameUnpaused += KitchenGameManager_OnGameUnpaused;

        Hide();
    }

    private void KitchenGameManager_OnGameUnpaused(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void KitchenGameManager_OnGamePaused(object sender, System.EventArgs e)
    {
        Show();
    }

    void Show()
    {
        gameObject.SetActive(true);

        resumeButton.Select();        //Selected by default, for controller support.
    }

    void Hide() => gameObject.SetActive(false);
}
