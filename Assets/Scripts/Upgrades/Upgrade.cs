using System;
using Player;
using StateMachineBehaviourScripts;
using UnityEngine;
using UnityEngine.Serialization;
using Utils.Entity;

namespace Upgrades {
    [Serializable] public abstract class Upgrade : ScriptableObject {
        [FormerlySerializedAs("name")] public string upgradeName;
    }
    
    public abstract class AttackUpgrade : Upgrade {
        public AttackAction attackAction;
        
        public abstract  void GetUpgrade(DamageInstance damageInstance);
    }
    
    public abstract class CharacterUpgrade : Upgrade {
        public abstract void GetUpgrade(PlayerScript playerScript);
    }
}