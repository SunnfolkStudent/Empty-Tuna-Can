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

    private Animator _animator;
    private TestMove _testMove;
    
    private void Awake() {
        _timer = new Timer(inputBuffer, false);
        _timer.OnComplete += ClearInputActions;
        _allComboActions = ScrubUtils.GetAllScrubsInResourceFolder<ComboAction>("ComboActions");

        _animator = GetComponent<Animator>();
        _testMove = GetComponent<TestMove>();
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

    private void CheckIfComboComplete(ComboAction combo) {
        if (combo.keyCombo.Length <= _currentIndex) {
            ExecuteComboAction(combo);
        }
    }

    private void ResetAvailableComboActions() {
        _availableCombos = _allComboActions.ToList();
    }

    private void ExecuteComboAction(ComboAction combo) {
        _animator.Play(combo.animation.name);
        _testMove.movementEnabled = false;
        Debug.Log("Combo Completed: " + combo.name + _inputActions.Aggregate("", (current, inputAction) => current +  " | " + inputAction.action.name));
        if (_currentIndex > 1) ClearInputActions();
        else RemoveComboAvailability(combo);
    }
}