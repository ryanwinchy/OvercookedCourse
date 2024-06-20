using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MusicManager : MonoBehaviour
{

    public static MusicManager Instance { get; private set; }

    AudioSource audioSource;
    public float Volume { get; private set; } = 0.3f;

    private void Awake()
    {
        Instance = this;

        audioSource = GetComponent<AudioSource>();

        Volume = PlayerPrefs.GetFloat("MusicVolume", 0.3f);         //Basic load.
        audioSource.volume = Volume;
    }
    public void ChangeVolume()
    {
        Volume += 0.1f;

        if (Volume > 1f)            //We cycle up in 0.1 increments, then back to 0.
            Volume = 0f;

        audioSource.volume = Volume;

        PlayerPrefs.SetFloat("MusicVolume", Volume);        //Basic save.
        PlayerPrefs.Save();
    }
}
