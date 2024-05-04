using System;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils.Singleton;

namespace Audio {
    public class AudioManager : PersistentSingleton<AudioManager>
    {
        private EventInstance menuMusicInstance;
        private EventInstance combatMusicInstance;
        private EventInstance bassCombatMusicInstance;

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
        public bool combatMusicPlaying;
        public bool bassCombatMusicPlaying;
        
        private Scene currentScene;
        private string currentSceneName;
        
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

            currentScene = SceneManager.GetActiveScene();
            currentSceneName = currentScene.name;
        }

        private void Update()
        {
            masterBus.setVolume(masterVolume);
            musicBus.setVolume(musicVolume);
            sfxBus.setVolume(sfxVolume);

            if (!combatMusicPlaying && SceneManager.GetSceneByName("Area1").isLoaded || !combatMusicPlaying && SceneManager.GetSceneByName("Area2").isLoaded)
            {
                StartCombatMusic(FmodEvents.Instance.CombatMusic);
            }
            else if (SceneManager.GetSceneByName("GameOver").isLoaded || SceneManager.GetSceneByName("WinScene").isLoaded || SceneManager.GetSceneByName("Area3").isLoaded || SceneManager.GetSceneByName("MainMenu").isLoaded)
            {
                StopCombatMusic();
            }
            
            if (!bassCombatMusicPlaying && SceneManager.GetSceneByName("Area3").isLoaded)
            {
                StartBassCombatMusic(FmodEvents.Instance.BassCombatMusic);
            }
            else if (SceneManager.GetSceneByName("GameOver").isLoaded || SceneManager.GetSceneByName("WinScene").isLoaded)
            {
                StopBassCombatMusic();
            }
            
            if (!menuMusicPlaying && SceneManager.GetSceneByName("MainMenu").isLoaded)
            {
                StartMenuMusic(FmodEvents.Instance.MenuMusic);;
            }
            else if (SceneManager.GetSceneByName("Area1").isLoaded || SceneManager.GetSceneByName("Area2").isLoaded || SceneManager.GetSceneByName("Area3").isLoaded)
            {
                StopMenuMusic();
            }
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

        public void StartCombatMusic(EventReference combatMusic)
        {
            combatMusicInstance = CreateEventInstance(combatMusic);
            combatMusicInstance.start();
            combatMusicPlaying = true;
        }

        public void StopCombatMusic()
        {
            combatMusicInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            combatMusicPlaying = false;
        }
        
        public void StartBassCombatMusic(EventReference bassCombatMusic)
        {
            bassCombatMusicInstance = CreateEventInstance(bassCombatMusic);
            bassCombatMusicInstance.start();
            bassCombatMusicPlaying = true;
        }

        public void StopBassCombatMusic()
        {
            bassCombatMusicInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            bassCombatMusicPlaying = false;
        }
    }
}
