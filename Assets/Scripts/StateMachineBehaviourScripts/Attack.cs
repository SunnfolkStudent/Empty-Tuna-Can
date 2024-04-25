using Entities;
using Entities.Player;
using UnityEngine;
using Utils.Entity;

namespace StateMachineBehaviourScripts {
    public class Attack : StateMachineBehaviour {
        public bool canMoveDuring;
        public int staggerResistanceBonusDuringAttack;
        public AttackAction attackAction;
        public DamageInstance damageInstance;
        
        [HideInInspector] public PlayerScript playerScript;
        
        private void Awake() {
            damageInstance.Initialize();
        }
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            playerScript.hitbox.damageInstance = damageInstance;
            playerScript.staggerResistance += staggerResistanceBonusDuringAttack;
        }
        
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            playerScript.staggerResistance -= staggerResistanceBonusDuringAttack;
        }
        
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            playerScript.movementEnabled = canMoveDuring;
        }
    }
    
    public enum AttackAction {
        LightAttack1, LightAttack2, LightAttack3, ForwardLightAttack1,
        HeavyAttack1, HeavyAttack2, HeavyAttack3, ForwardHeavyAttack1,
    }
}