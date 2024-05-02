using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using StateMachineBehaviourScripts;
using UnityEngine;
using Utils;

namespace Entities.Player {
    public class ActionManager : MonoBehaviour {
        [SerializeField] private float inputBuffer = 0.25f;
    
        private Timer _combatInputTimer;
        [SerializeField] private CombatAction[] moveList;
        private List<CombatActionInstance> _availableCombatActions;
        [CanBeNull] private CombatActionInstance _queuedCombatAction;
    
        [Serializable] private class CombatActionInstance {
            internal readonly CombatInput[] CombatInputs;
            internal readonly CombatOutput CombatOutput;
            internal int Index;

            public CombatActionInstance(CombatAction combatAction) {
                CombatInputs = combatAction.combatInputs;
                CombatOutput = combatAction.combatOutput;
                Index = 0;
            }
        }
    
        [Header("References")]
        [SerializeField] private Animator animator;
        [SerializeField] private PlayerScript playerScript;

        private static readonly int CombatOutputAnimatorParameter = Animator.StringToHash("CombatOutput");

        private void Awake() {
            _combatInputTimer = new Timer(inputBuffer, false);
            _combatInputTimer.OnComplete += ResetCombatActions;
            moveList = ScrubUtils.GetAllScrubsInResourceFolder<CombatAction>("ComboActions/TestPlayer"); // TODO: Comment out
        
            _availableCombatActions = moveList.Select(combatAction => new CombatActionInstance(combatAction)).ToList();

            foreach (var stateBehaviour in animator.GetBehaviours<StateBehaviour>()) {
                stateBehaviour.actionManager = this;
            }
        }
    
        private void ResetCombatActions() {
            foreach (var combatAction in _availableCombatActions) {
                combatAction.Index = 0;
            }
        }
   
        public void ReceiveCombatInput(CombatInput combatInput) {
            if (combatInput == CombatInput.None || PlayerScript.Paused) return;
        
            _combatInputTimer.StopTimer();
            _combatInputTimer.StartTimer(inputBuffer);
        
            foreach (var combatAction in _availableCombatActions) {
                if (combatAction.CombatInputs.Length == 0) continue;
                if (combatAction.CombatInputs[combatAction.Index] != combatInput) continue;
                combatAction.Index++;
            
                if (combatAction.Index < combatAction.CombatInputs.Length) continue;
                SetNextCombatAction(combatAction);
            }
        }
    
        private void SetNextCombatAction(CombatActionInstance combatAction) {
            combatAction.Index = 0;
            if (_queuedCombatAction != null && _queuedCombatAction.CombatInputs.Length > combatAction.CombatInputs.Length) return;
            _queuedCombatAction = combatAction;
            animator.SetInteger(CombatOutputAnimatorParameter, (int)combatAction.CombatOutput);
        }

        public void ResetInteger() {
            animator.SetInteger(CombatOutputAnimatorParameter, 0);
            _queuedCombatAction = null;
        }

        public void OnActionOver() {
            playerScript.CheckIfFlipObject();
            _queuedCombatAction = null;
            playerScript.canMove = true;
        }
    }

    public enum CombatInput {
        None, LightAttack, HeavyAttack,
        Forward, Up, UpDiagonal,
        Down, DownDiagonal,
        Jump
    }

    public enum CombatOutput {
        None, LightAttack, HeavyAttack,
        DashLightAttack, DashUp, DashDown, DashForward,
        QuarterCircleForwardLightAttack, Jump
    }
}