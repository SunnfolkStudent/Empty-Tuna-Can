using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using Utils.Singleton;

namespace Audio {
    public class AudioManager : PersistentSingleton<AudioManager>
    {
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
}
