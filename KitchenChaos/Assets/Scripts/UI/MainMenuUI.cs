using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{

    [SerializeField] Button playButton;
    [SerializeField] Button quitButton;

    private void Awake()
    {
        playButton.onClick.AddListener(() =>               //On button click event. Same as setting in editor. This is a lambda expression.
        {
            Loader.Load(Loader.Scene.GameScene);
        });
        quitButton.onClick.AddListener(() =>               //On button click event. Same as setting in editor. This is a lambda expression.
        {
            Application.Quit();    //Quits game, once game built.
        });

        Time.timeScale = 1f;          //So if you had it paused then restart, it defaults to normal speed (unpauses).


    }




}
