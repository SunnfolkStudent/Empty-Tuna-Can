using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using Utils.Singleton;

public class FmodEvents : PersistentSingleton<FmodEvents>
{ 
  // public static FmodEvents Instance { get; private set; }

   #region OneShotEvents

   [field: Header("GetHit")]
   [field: SerializeField] public EventReference GetHit { get; private set; }
   
   

   #endregion
   // private void Awake()
   // {
   //    if (Instance != null)
   //    {
   //       Debug.Log("Found more than one FMOD events in instance");
   //       Instance = this;
   //    }
   // }

   private void Update()
   {
      if (Input.GetKeyDown("l"))
      {
         AudioManager.Instance.PlayOneShot(GetHit, this.transform.position);
      }
   }
}
