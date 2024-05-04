using System;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using Utils.Singleton;

namespace Audio {
    public class AudioManager : PersistentSingleton<AudioManager>
    {
        private EventInstance menuMusicInstance;

        [Header("Volume")] 
        [Range(0, 1)]
        public float masterVolume = 1;
        [Range(0, 1)]
        public float sfxVolume = 1;
        [Range(0, 1)]
        public float musicVolume = 1;

        private Bus masterBus;
        private Bus musicBus;
        private Bus sfxBus;

        public bool menuMusicPlaying;
        protected override void Awake()
        {
            base.Awake();
            masterBus = RuntimeManager.GetBus("bus:/");
            musicBus = RuntimeManager.GetBus("bus:/Music");
            sfxBus = RuntimeManager.GetBus("bus:/SFX");
        }

        private void Start()
        {
            if (!PlayerPrefs.HasKey("MasterVolume"))
            {
                PlayerPrefs.SetFloat("MasterVolume", 1);
                PlayerPrefs.SetFloat("SFXVolume", 1);
                PlayerPrefs.SetFloat("MusicVolume", 1);
            }
            masterVolume = PlayerPrefs.GetFloat("MasterVolume");
            sfxVolume = PlayerPrefs.GetFloat("SFXVolume"); 
            musicVolume = PlayerPrefs.GetFloat("MusicVolume");
        }

        private void Update()
        {
            masterBus.setVolume(masterVolume);
            musicBus.setVolume(musicVolume);
            sfxBus.setVolume(sfxVolume);
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
            menuMusicPlaying = true;

        }
        public void StopMenuMusic()
        {
            menuMusicInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            menuMusicPlaying = false;
        }
    }
}
