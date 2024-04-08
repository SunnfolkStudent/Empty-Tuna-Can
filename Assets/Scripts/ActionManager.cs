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
        internal int Index;
        
        public CombatActionInstance(CombatAction combatAction) {
            CombatInputs = combatAction.combatInputs;
            Animation = combatAction.animation;
            Index = 0;
        }
    }
    
    private Animator _animator;
    private PlayerScript _playerScript;
    
    private void Awake() {
        _animator = GetComponent<Animator>();
        _playerScript = GetComponent<PlayerScript>();
        
        _combatInputTimer = new Timer(inputBuffer, false);
        _combatInputTimer.OnComplete += ResetCombatActions;
        _allCombatActions = ScrubUtils.GetAllScrubsInResourceFolder<CombatAction>("ComboActions")
            .Select(combatAction => new CombatActionInstance(combatAction)).ToArray();

        _availableCombatActions = _allCombatActions.ToList();
    }
    
    public void ReceiveCombatInput(CombatInput combatInput) {
        if (combatInput == CombatInput.None) return;
        
        // Debug.Log($"ReceiveCombatInput: {combatInput}");
        
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
            if (_animator.GetCurrentAnimationClip().name is "Idle" or "Walk") {
                PlayNextCombatAction();
            }
        }
    }
    
    private void PlayNextCombatAction() {
        if (_nextCombatAction == null) return;
        
        Debug.Log("Executing combat action " + _nextCombatAction.Animation.name);
        
        _playerScript.movementEnabled = false;
        _animator.Play(_nextCombatAction.Animation.name);

        // _availableCombatActions = _nextCombatAction.ChainActions.ToList();
            
        _nextCombatAction = null;
    }
    
    private void OnAttackOver() {
        if (_nextCombatAction != null) {
            PlayNextCombatAction();
        }
        else {
            _animator.Play("Idle");
            _playerScript.movementEnabled = true;
        }
    }
}

public enum CombatInput {
    None,
    LightAttack, HeavyAttack,
    Forward,
    Up, UpDiagonal,
    Down, DownDiagonal
}