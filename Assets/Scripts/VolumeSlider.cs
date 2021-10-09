using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeSlider : MonoBehaviour
{
    public void SetVolume(float sliderValue)
    {
        SettingsData.current.volume = sliderValue;
        Debug.Log(SettingsData.current.volume);
    }
}
