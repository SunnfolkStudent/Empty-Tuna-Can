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
        // internal readonly AnimationClip Animation;
        internal readonly CombatOutput CombatOutput;
        internal readonly bool CanMoveDuring;
        internal int Index;

        public CombatActionInstance(CombatAction combatAction) {
            CombatInputs = combatAction.combatInputs;
            // Animation = combatAction.animation;
            CombatOutput = combatAction.combatOutput;
            CanMoveDuring = combatAction.canMoveDuring;
            Index = 0;
        }
    }
    
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerScript playerScript;

    private static readonly int Output = Animator.StringToHash("CombatOutput");

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
        Debug.Log(combatAction.CombatOutput);
        combatAction.Index = 0;
        if (_queuedCombatAction == null || _queuedCombatAction.CombatInputs.Length <= combatAction.CombatInputs.Length) {
            _queuedCombatAction = combatAction;
            animator.SetInteger(Output, (int)combatAction.CombatOutput);
            playerScript.movementEnabled = _queuedCombatAction.CanMoveDuring;
            playerScript.inAction = true;
        }
    }
    
    private void ResetInteger() {
        animator.SetInteger(Output, 0);
        _queuedCombatAction = null;
    }
    
    private void OnActionOver() {
        playerScript.CheckIfFlipObject();
        playerScript.movementEnabled = true;
        playerScript.inAction = false;
        _queuedCombatAction = null;
    }
}

public enum CombatInput {
    None,
    LightAttack, HeavyAttack,
    Forward,
    Up, UpDiagonal,
    Down, DownDiagonal,
    Jump
}

public enum CombatOutput {
    None,
    LightAttack, HeavyAttack,
    DashLightAttack, DashUp, DashDown, DashForward,
    QuarterCircleForwardLightAttack,
    Jump, XXY
}