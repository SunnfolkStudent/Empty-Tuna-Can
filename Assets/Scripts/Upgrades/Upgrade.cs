using System;
using Player;
using StateMachineBehaviourScripts;
using UnityEngine;
using Utils.Entity;

namespace Upgrades {
    [Serializable] public abstract class Upgrade : ScriptableObject {
        public string name;
    }
    
    public abstract class AttackUpgrade : Upgrade {
        public AttackAction attackAction;
        
        public abstract  void GetUpgrade(DamageInstance damageInstance);
    }
    
    public abstract class CharacterUpgrade : Upgrade {
        public abstract void GetUpgrade(PlayerScript playerScript);
    }
}