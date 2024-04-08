using System;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using Utils;

public class ActionManager : MonoBehaviour {
    [SerializeField] private float inputBuffer = 0.25f;
    
    private Timer _combatInputTimer;
    private int _currentIndex;
    private CombatActionInstance[] _allCombatActions;
    [CanBeNull] private CombatActionInstance _nextCombatAction;
    
    private Timer _directionInputTimer;
    private DirectionActionInstance[] _allDirectionActions;
    [CanBeNull] private DirectionActionInstance _nextDirectionAction;
    
    private Animator _animator;
    private PlayerScript _playerScript;
    
    private void Awake() {
        _animator = GetComponent<Animator>();
        _playerScript = GetComponent<PlayerScript>();
        
        _combatInputTimer = new Timer(inputBuffer, false);
        _combatInputTimer.OnComplete += ClearInputActions;
        _allCombatActions = ScrubUtils.GetAllScrubsInResourceFolder<ComboAction>("ComboActions")
            .Select(combatAction => new CombatActionInstance(combatAction)).ToArray();
        
        _directionInputTimer = new Timer(inputBuffer, false);
        _directionInputTimer.OnComplete += ResetDirectionActions;
        _allDirectionActions = ScrubUtils.GetAllScrubsInResourceFolder<DirectionAction>("DirectionalAction")
            .Select(directionAction => new DirectionActionInstance(directionAction)).ToArray();
    }
    
    public void ReceiveCombatInput(CombatInput combatInput) {
        Debug.Log($"ReceiveCombatInput: {combatInput}");
        _combatInputTimer.StopTimer();
        _combatInputTimer.StartTimer(inputBuffer);
        
        foreach (var combatAction in _allCombatActions) {
            if (combatAction.CombatInputs.Length == 0) continue;
            if (combatAction.CombatInputs[combatAction.Index] != combatInput) continue;
            combatAction.Index++;
            
            if (combatAction.Index < combatAction.CombatInputs.Length) continue;
            SetNextCombatAction(combatAction);
        }
    }
    
    [Serializable] private class CombatActionInstance {
        internal readonly CombatInput[] CombatInputs;
        internal readonly  AnimationClip Animation;
        internal int Index;
        
        public CombatActionInstance(ComboAction comboAction) {
            CombatInputs = comboAction.combatInputs;
            Animation = comboAction.animation;
            Index = 0;
        }
    }
    
    private void ClearInputActions() {
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
        
        _playerScript.movementEnabled = false;
        _animator.Play(_nextCombatAction.Animation.name);
        _nextCombatAction = null;
    }
    
    private void AttackOver(string animationName) {
        if (_nextCombatAction != null) {
            PlayNextCombatAction();
        }
        else {
            _animator.Play("Idle");
            _playerScript.movementEnabled = true;
        }
    }
    
    #region ---Direction---
    private class DirectionActionInstance {
        internal readonly DirectionalInputManager.Direction[] DirectionInputs;
        internal readonly CombatInput CombatInput;
        internal int Index;
        
        public DirectionActionInstance(DirectionAction directionAction) {
            DirectionInputs = directionAction.directionInputs;
            CombatInput = directionAction.combatInput;
            Index = 0;
        }
    }
    
    public void ReceiveDirection(DirectionalInputManager.Direction direction) {
        _directionInputTimer.StopTimer();
        _directionInputTimer.StartTimer(inputBuffer);
        
        
        foreach (var directionAction in _allDirectionActions) {
            if (directionAction.DirectionInputs.Length == 0) continue;
            if (directionAction.DirectionInputs[directionAction.Index] != direction) continue;
            directionAction.Index++;
            
            if (directionAction.Index < directionAction.DirectionInputs.Length) continue;
            directionAction.Index = 0;

            if (_nextDirectionAction != null && _nextDirectionAction.DirectionInputs.Length >= directionAction.DirectionInputs.Length) continue;
            _nextDirectionAction = directionAction;
        }

        if (_nextDirectionAction == null) return;
        ReceiveCombatInput(_nextDirectionAction.CombatInput);
        _nextDirectionAction = null;
    }

    private void ResetDirectionActions() {
        foreach (var directionAction in _allDirectionActions) {
            directionAction.Index = 0;
        }
    }
    #endregion
}

public enum CombatInput {
    LightAttack, HeavyAttack,
    Forward, Up, 
    Down, DownDiagonal,
    QuarterCircle
}