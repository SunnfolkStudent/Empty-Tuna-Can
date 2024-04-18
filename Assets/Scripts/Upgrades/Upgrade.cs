using StateMachineBehaviourScripts;
using UnityEngine;
using Utils.Entity;

namespace Upgrades {
    public class Upgrade : ScriptableObject {
        public string text;
        public AttackAction attackAction;

        public virtual void GetUpgrade(DamageInstance damageInstance) {
        }
    }
}