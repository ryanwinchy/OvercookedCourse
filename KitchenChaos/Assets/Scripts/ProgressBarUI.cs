using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] CuttingCounter cuttingCounter;
    [SerializeField] Image barImage;

    private void Start()
    {
        cuttingCounter.OnCuttingProgressChanged += CuttingCounter_OnCuttingProgressChanged;

        barImage.fillAmount = 0;
        Hide();
    }

    private void CuttingCounter_OnCuttingProgressChanged(object sender, CuttingCounter.OnProgressChangedEventArgs e)
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
