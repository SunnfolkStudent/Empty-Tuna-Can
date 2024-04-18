using Player;
using UnityEngine;
using Utils.Entity;

namespace StateMachineBehaviourScripts {
    public class Attack : StateMachineBehaviour {
        public bool canMoveDuring;
        public AttackAction attackAction;
        public DamageInstance damageInstance;
        
        [HideInInspector] public PlayerScript playerScript;
        
        private void Awake() {
            damageInstance.Initialize();
        }
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            playerScript.hitbox.damageInstance = damageInstance;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            base.OnStateUpdate(animator, stateInfo, layerIndex);
            playerScript.movementEnabled = canMoveDuring;
        }
    }
    
    public enum AttackAction{ LightAttack1, LightAttack2, LightAttack3, ForwardLightAttack1 }
}