using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public MusicManagerInstance instance;
    public AudioSource audioSource;
    
    // Start is called before the first frame update
    void OnEnable()
    {
        if (!instance.RegisterInstance(this))
            Destroy(gameObject);
    }

    private void OnDisable()
    {
        instance.UnregisterInstance(this);
    }

    public void ChangeMusic(AudioClip music)
    {
        audioSource.clip = music;
    }
}
