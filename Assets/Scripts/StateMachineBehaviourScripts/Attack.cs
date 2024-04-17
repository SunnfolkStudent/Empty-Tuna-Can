using UnityEngine;
using Utils.Entity;

namespace StateMachineBehaviourScripts {
    public class Attack : StateMachineBehaviour {
        [SerializeField] private DamageInstance damageInstance;
        
        [HideInInspector] public PlayerScript playerScript;

        private void Awake() {
            damageInstance.Initialize();
        }
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            playerScript.hitbox.damageInstance = damageInstance;
        }
        
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            playerScript.hitbox.damageInstance = null;
        }
    }
}