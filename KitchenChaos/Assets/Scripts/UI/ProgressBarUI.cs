using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] GameObject iHasProgressGameObject;
    [SerializeField] Image barImage;

    IHasProgress iHasProgress;

    private void Start()
    {
        iHasProgress = iHasProgressGameObject.GetComponent<IHasProgress>();

        if (iHasProgress == null )
        {
            Debug.LogError("Game Object " + iHasProgressGameObject + " does not have a component that implements IHasProgress");
        }

        iHasProgress.OnProgressChanged += IHasProgress_OnCuttingProgressChanged;

        barImage.fillAmount = 0;
        Hide();
    }

    private void IHasProgress_OnCuttingProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        barImage.fillAmount = e.progressNormalized;

        if (e.progressNormalized == 0f || e.progressNormalized == 1f)       //if bar full or empty.
            Hide();
        else
            Show();
    }

    void Show() => gameObject.SetActive(true);

    void Hide() => gameObject.SetActive(false);
}
