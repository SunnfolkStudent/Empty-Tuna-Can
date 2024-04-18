using StateMachineBehaviourScripts;
using UnityEngine;
using Utils.Entity;

namespace Upgrades {
    public class Upgrade : ScriptableObject {
        public string name;
    }

    public class AttackUpgrade : Upgrade {
        public AttackAction attackAction;

        public virtual void GetUpgrade(DamageInstance damageInstance) {
        }
    }
    
    public class CharacterUpgrade : Upgrade {
        public virtual void GetUpgrade(PlayerScript playerScript) {
        }
    }
}