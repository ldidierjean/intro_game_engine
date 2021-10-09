using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsData : MonoBehaviour
{
    public static SettingsData current;
    public float volume;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        current = this;
    }
}
