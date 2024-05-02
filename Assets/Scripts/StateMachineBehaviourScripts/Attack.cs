using Entities;
using UnityEngine;

namespace StateMachineBehaviourScripts {
    public class Attack : StateMachineBehaviour {
        public bool canMoveDuring;
        public int staggerResistanceBonusDuringAttack;
        public AttackAction attackAction;
        public DamageInstance damageInstance;
        
        [HideInInspector] public Damageable damageable;
        
        private void Awake() {
            damageInstance.Initialize();
        }
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            damageable.hitbox.damageInstance = damageInstance;
            damageable.staggerResistance += staggerResistanceBonusDuringAttack;
        }
        
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            damageable.staggerResistance -= staggerResistanceBonusDuringAttack;
        }
        
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            damageable.canMove = canMoveDuring;
        }
    }
    
    public enum AttackAction {
        LightAttack1, LightAttack2, LightAttack3, ForwardLightAttack1,
        HeavyAttack1, HeavyAttack2, HeavyAttack3, ForwardHeavyAttack1,
    }
}