using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class optionsSettings : MonoBehaviour
{

    private float musicVolume;
    private float soundEffectsVolume;

    private float sensitivity;


    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
    }

    public void SetSoundEffectsVolume(float volume)
    {
        soundEffectsVolume = volume;
    }
}
