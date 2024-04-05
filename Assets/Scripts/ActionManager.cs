using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

public class ActionManager : MonoBehaviour {
    [SerializeField] private float inputBuffer = 0.25f;
    
    private List<InputAction.CallbackContext> _inputActions = new ();
    private Timer _timer;
    
    private int _currentIndex;
    
    private ComboAction[] _allComboActions;
    private List<ComboAction> _availableCombos = new();

    [SerializeField] private ComboAction nextComboAction;

    private Animator _animator;
    private PlayerScript _playerScript;
    
    private void Awake() {
        _timer = new Timer(inputBuffer, false);
        _timer.OnComplete += ClearInputActions;
        _allComboActions = ScrubUtils.GetAllScrubsInResourceFolder<ComboAction>("ComboActions");

        _animator = GetComponent<Animator>();
        _playerScript = GetComponent<PlayerScript>();
    }

    private void Start() {
        ResetAvailableComboActions();
    }

    public void ReceiveAction(InputAction.CallbackContext inputAction) {
        var combosToRemove = _availableCombos.Where(combo => combo.keyCombo[_currentIndex] != inputAction.action.name).ToArray();

        foreach (var comboAction in combosToRemove) {
            RemoveComboAvailability(comboAction);
        }
        
        if (_availableCombos.Count == 0) {
            ClearInputActions();
            return;
        }
        
        _currentIndex++;
        
        _inputActions.Add(inputAction);
        _timer.StopTimer();
        _timer.StartTimer(inputBuffer);
        
        foreach (var combo in _availableCombos.Where(combo => combo.keyCombo.Length <= _currentIndex)) {
            ExecuteComboAction(combo);
            return;
        }
    }

    private void ClearInputActions() {
        ResetAvailableComboActions();
        _inputActions.Clear();
        _timer.StopTimer();
        _currentIndex = 0;
    }

    private void RemoveComboAvailability(ComboAction combo) {
        _availableCombos.Remove(combo);
    }

    private void ResetAvailableComboActions() {
        _availableCombos = _allComboActions.ToList();
    }

    private void ExecuteComboAction(ComboAction combo) {
        if (nextComboAction == null || nextComboAction.keyCombo.Length < combo.keyCombo.Length) {
            nextComboAction = combo;
            StartCoroutine(WaitTilCanAttack());
        }
        
        Debug.Log("Combo Completed: " + combo.name + _inputActions.Aggregate("", (current, inputAction) => current +  " | " + inputAction.action.name));
        RemoveComboAvailability(combo);
    }
    
    private IEnumerator WaitTilCanAttack() {
        while (_animator.GetCurrentAnimationClip().name is not ("Idle" or "Walk")) {
            yield return null;
        }
        
        if (nextComboAction != null) {
            PlayNextComboAction();
        }
    }

    private void PlayNextComboAction() {
        _animator.Play(nextComboAction.animation.name);
        _playerScript.movementEnabled = nextComboAction.canMoveDuring;
        nextComboAction = null;
    }
    
    private void AttackOver(string animationName) {
        Debug.Log("Attack is over");
        
        if (nextComboAction != null) {
            PlayNextComboAction();
        }
        else {
            _animator.Play("Idle");
            _playerScript.movementEnabled = true;
        }
        
    }
}