using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using Utils.Singleton;

namespace Audio {
   public class FmodEvents : PersistentSingleton<FmodEvents>
   {
     
      [field: Header("OneShots")]
      [field: SerializeField] public EventReference GetHit { get; private set; }
   
      [field: Header("EventInstances")]
      [field: SerializeField] public EventReference MenuMusic { get; private set; }
      [field: SerializeField] public EventReference CombatMusic { get; private set; }
      private void Update()
      {
         if (Input.GetKeyDown("l"))
         {
            AudioManager.Instance.PlayOneShot(GetHit, this.transform.position);
         }
      }

      
   }
}
