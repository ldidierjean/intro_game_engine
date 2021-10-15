using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeSlider : MonoBehaviour
{
    public void SetMusicVolume(float sliderValue)
    {
        SettingsData.current.musicVolume = sliderValue;
    }

    public void SetSoundvolume(float sliderValue)
    {
        SettingsData.current.soundvolume = sliderValue;
    }
}
