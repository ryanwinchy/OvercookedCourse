using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField] GameObject stoveOnGameObject;
    [SerializeField] GameObject particlesGameObject;

    [SerializeField] StoveCounter stoveCounter;

    private void Start()
    {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEvent e)
    {
        bool showVisual = e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried;    //True if in cooking states.

        stoveOnGameObject.SetActive(showVisual);
        particlesGameObject.SetActive(showVisual);
    }
}
