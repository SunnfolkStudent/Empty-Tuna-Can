using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using Utils.Singleton;

public class AudioManager : PersistentSingleton<AudioManager>
{
   
    // public static AudioManager Instance { get; private set; }
    //
    // private void Awake()
    // {
    //     if (Instance != null)
    //     {
    //         Debug.LogError("Found more than one audio manager in the scene");
    //     }
    //
    //     Instance = this;
    // }

    public void PlayOneShot(EventReference sound, Vector3 worldpos)
    {
        RuntimeManager.PlayOneShot(sound, worldpos);
    }

    public EventInstance CreateEventInstance(EventReference eventReference)
    {
        EventInstance eventinstance = RuntimeManager.CreateInstance(eventReference);
        return eventinstance;
    }
}
