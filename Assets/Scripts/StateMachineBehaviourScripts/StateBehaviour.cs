using Entities.Player;
using UnityEngine;

namespace StateMachineBehaviourScripts {
    public class StateBehaviour : StateMachineBehaviour {
        [HideInInspector] public ActionManager actionManager;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            actionManager.ResetInteger();
        }
        
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            actionManager.OnActionOver();
        }
    }
}