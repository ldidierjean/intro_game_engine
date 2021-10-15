using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsData : MonoBehaviour
{
    public enum Difficulty
    {
        Easy, Medium, Hard
    };

    public static SettingsData current;
    public float musicVolume;
    public float soundvolume;
    public Difficulty difficulty;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        current = this;
    }
}
