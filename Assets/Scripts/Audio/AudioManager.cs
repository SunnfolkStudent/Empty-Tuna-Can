using System;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using Utils.Singleton;

namespace Audio {
    public class AudioManager : PersistentSingleton<AudioManager>
    {
        private EventInstance menuMusicInstance;

        private void Start()
        {
            StartMenuMusic(FmodEvents.Instance.MenuMusic);
        }

        public void PlayOneShot(EventReference sound, Vector3 worldpos)
        {
            RuntimeManager.PlayOneShot(sound, worldpos);
        }

        public EventInstance CreateEventInstance(EventReference eventReference)
        {
            EventInstance eventinstance = RuntimeManager.CreateInstance(eventReference);
            return eventinstance;
        }
        
        public void StartMenuMusic(EventReference MenuMusic)
        {
            menuMusicInstance = CreateEventInstance(MenuMusic);
            menuMusicInstance.start();
        }
        public void StopMenuMusic()
        {
            menuMusicInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
    }
}
