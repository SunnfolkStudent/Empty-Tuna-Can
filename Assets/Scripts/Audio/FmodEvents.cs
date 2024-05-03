using FMODUnity;
using UnityEngine;
using Utils.Singleton;

namespace Audio {
   public class FmodEvents : PersistentSingleton<FmodEvents>
   {
      #region OneShotEvents
      [field: Header("GetHit")]
      [field: SerializeField] public EventReference GetHit { get; private set; }
      #endregion

      private void Update()
      {
         if (Input.GetKeyDown("l"))
         {
            AudioManager.Instance.PlayOneShot(GetHit, this.transform.position);
         }
      }
   }
}
