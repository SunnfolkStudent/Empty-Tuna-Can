using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using Utils;

public class ActionManager : MonoBehaviour {
    [SerializeField] private float inputBuffer = 0.25f;
    
    private Timer _combatInputTimer;
    [SerializeField] private CombatAction[] moveList;
    private List<CombatActionInstance> _availableCombatActions;
    [CanBeNull] private CombatActionInstance _queuedCombatAction;
    
    [Serializable] private class CombatActionInstance {
        internal readonly CombatInput[] CombatInputs;
        internal readonly CombatOutput CombatOutput;
        internal readonly bool CanMoveDuring;
        internal int Index;

        public CombatActionInstance(CombatAction combatAction) {
            CombatInputs = combatAction.combatInputs;
            CombatOutput = combatAction.combatOutput;
            CanMoveDuring = combatAction.canMoveDuring;
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
        moveList = ScrubUtils.GetAllScrubsInResourceFolder<CombatAction>("ComboActions/TestPlayer");
        
        _availableCombatActions = moveList.Select(combatAction => new CombatActionInstance(combatAction)).ToList();
    }
    
    private void ResetCombatActions() {
        foreach (var combatAction in _availableCombatActions) {
            combatAction.Index = 0;
        }
    }
   
    public void ReceiveCombatInput(CombatInput combatInput) {
        if (combatInput == CombatInput.None) return;
        
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
        playerScript.movementEnabled = _queuedCombatAction.CanMoveDuring;
    }
    
    private void ResetInteger() {
        animator.SetInteger(CombatOutputAnimatorParameter, 0);
        _queuedCombatAction = null;
    }
    
    private void OnActionOver() {
        playerScript.CheckIfFlipObject();
        _queuedCombatAction = null;
        
        if (animator.GetCurrentAnimationClip().name is not ("Idle" or "Walk") || animator.GetInteger(CombatOutputAnimatorParameter) != 0) return;
        playerScript.movementEnabled = true;
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