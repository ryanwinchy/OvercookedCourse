using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenGameManager : MonoBehaviour
{

    public static KitchenGameManager Instance { get; private set; }

    public event EventHandler OnStateChanged;

    public event EventHandler OnGamePaused;       //For pause menu UI.
    public event EventHandler OnGameUnpaused;
    enum State { WaitingToStart, CountdownToStart, GamePlaying, GameOver }

    State state;

    float waitingToStartTimer = 1f;
    float countdownToStartTimer = 3f;
    float gamePlayingTimer;
    float gamePlayingTimerMax = 10f;

    bool isGamePaused = false;

    private void Awake()
    {
        Instance = this;

        state = State.WaitingToStart;
    }

    private void Start()
    {
        InputManager.Instance.OnPausePressed += InputManager_OnPausePressed;      //On pause pressed event.
    }

    private void InputManager_OnPausePressed(object sender, EventArgs e)
    {
        TogglePauseGame();
    }

    private void Update()
    {
        switch (state)
        {
            case State.WaitingToStart:
                waitingToStartTimer -= Time.deltaTime;
                if (waitingToStartTimer < 0)
                {
                    state = State.CountdownToStart;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.CountdownToStart:
                countdownToStartTimer -= Time.deltaTime;
                if (countdownToStartTimer < 0)
                {
                    state = State.GamePlaying;
                    gamePlayingTimer = gamePlayingTimerMax;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GamePlaying:
                gamePlayingTimer -= Time.deltaTime;
                if (gamePlayingTimer < 0)
                {
                    state = State.GameOver;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GameOver:
                break;
        }
    }



    public bool IsGamePlaying() => state == State.GamePlaying;      //Return if game is playing (not game over). So we can stop player interactions and such.
    public bool IsGameOver() => state == State.GameOver;        //Could avoid all these if just made state a property with {get; private set} , probably easier and cleaner.

    public bool IsCountDownToStartActive() => state == State.CountdownToStart;

    public float GetCountdownToStartTimer() => countdownToStartTimer;

    public float GetGamePlayingTimerNormalized() => 1 - (gamePlayingTimer / gamePlayingTimerMax);       //1 - because we are counting down not up. but we want turn timer to go up (fill in).


    public void TogglePauseGame()
    {
        isGamePaused = !isGamePaused;

        if (isGamePaused)
        {
            Time.timeScale = 0f;          //The logic in our game is all time.DeltaTime. This time scale is a multiplier for that, so 0 pauses all time.
            OnGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1f;
            OnGameUnpaused?.Invoke(this, EventArgs.Empty);
        }
    }





}
