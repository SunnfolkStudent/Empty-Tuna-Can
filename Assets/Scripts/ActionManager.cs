using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using Utils;

public class ActionManager : MonoBehaviour {
    [SerializeField] private float inputBuffer = 0.25f;
    
    private Timer _combatInputTimer;
    private CombatActionInstance[] _allCombatActions;
    private List<CombatActionInstance> _availableCombatActions;
    [CanBeNull] private CombatActionInstance _nextCombatAction;
    
    [Serializable] private class CombatActionInstance {
        internal readonly CombatInput[] CombatInputs;
        internal readonly AnimationClip Animation;
        internal readonly bool CanMoveDuring;
        internal int Index;

        public CombatActionInstance(CombatAction combatAction) {
            CombatInputs = combatAction.combatInputs;
            Animation = combatAction.animation;
            CanMoveDuring = combatAction.canMoveDuring;
            Index = 0;
        }
    }
    
    [Header("Script References")]
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerScript playerScript;
    
    [Header("RuntimeAnimatorControllers")] [Tooltip("Default controller is index 0")]
    [SerializeField] private RuntimeAnimatorController[] allControllers;
    
    private void Awake() {
        _combatInputTimer = new Timer(inputBuffer, false);
        _combatInputTimer.OnComplete += ResetCombatActions;
        _allCombatActions = ScrubUtils.GetAllScrubsInResourceFolder<CombatAction>("ComboActions")
            .Select(combatAction => new CombatActionInstance(combatAction)).ToArray();
        
        _availableCombatActions = _allCombatActions.ToList();
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
    
    private void ResetCombatActions() {
        foreach (var combatAction in _allCombatActions) {
            combatAction.Index = 0;
        }
    }
    
    private void SetNextCombatAction(CombatActionInstance combatAction) {
        combatAction.Index = 0;
        if (_nextCombatAction == null || _nextCombatAction.CombatInputs.Length <= combatAction.CombatInputs.Length) {
            _nextCombatAction = combatAction;
            if (animator == null) Debug.Log("no animator");
            else if (animator.GetCurrentAnimationClip().name is "Idle" or "Walk") {
                PlayNextCombatAction();
            }
        }
    }
    
    private void PlayNextCombatAction() {
        if (_nextCombatAction == null) return;
        
        Debug.Log("Executing combat action " + _nextCombatAction.Animation.name);
        
        playerScript.movementEnabled = _nextCombatAction.CanMoveDuring;
        playerScript.isInAction = true;
        
        animator.Play(_nextCombatAction.Animation.name);
        
        _nextCombatAction = null;
    }

    private void SetAnimationController(int index) {
        animator.runtimeAnimatorController = allControllers[index];
    }
    
    private void OnActionOver(int index) {
        SetAnimationController(index);
        
        if (_nextCombatAction != null) {
            PlayNextCombatAction();
            playerScript.CheckIfFlipObject();
        }
        else {
            animator.Play("Idle");
            playerScript.movementEnabled = true;
            playerScript.isInAction = false;
        }
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