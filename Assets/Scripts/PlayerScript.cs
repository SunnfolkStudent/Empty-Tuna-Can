using System;
using System.Collections.Generic;
using Items;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;
using Utils.Entity;

public class PlayerScript : Damageable {
    [SerializeField] private Transform projectileSpawnPos;
    
    [HideInInspector] public Vector2 moveVector;
    [HideInInspector] public bool movementEnabled = true;
    [HideInInspector] public bool isInAction;
    
    public List<Item> itemInventory;
    
    #region ---SelectedItem---
    private Item _selectedItem;
    
    public Item SelectedItem {
        set {
            _selectedItem = value;
            OnSelectItemChanged?.Invoke(_selectedItem != null ? _selectedItem : null);
        }
        get => _selectedItem;
    }
    
    public event Action<Item> OnSelectItemChanged;
    
    private int _selectedItemIndex;
    #endregion
    
    [Header("Script References")]
    [SerializeField] private Animator animator;
    [SerializeField] private ActionManager actionManager;
    [SerializeField] private EntityMovement entityMovement;
    private Transform _transform;
    
    private CombatInput _currentDirection;
    
    private void Awake() {
        _transform = transform;
        entityMovement = GetComponent<EntityMovement>();
    }
    
    private void Start() {
        SelectedItem = itemInventory[0];
        OnDying += OnDeath;
        
        PlayerUIFactory.CreatePlayerUI(this);
        
        teamNumber = FindObjectsByType<PlayerScript>(FindObjectsSortMode.None).Length;
        
        hitbox.teamNumber = teamNumber;
    }
    
    #region ---OnInputAction---
    #region ***---Movement---
    public void OnMove(InputAction.CallbackContext ctx) {
        moveVector = ctx.ReadValue<Vector2>();
        
        if (moveVector == Vector2.zero) {
            _currentDirection = CombatInput.None;
        }
        else {
            var o = DirectionalInputManager.DirectionFormVector2(moveVector);
            if (_currentDirection == o) return;
            _currentDirection = o;
            actionManager.ReceiveCombatInput(_currentDirection);
        }
    }

    public void OnJump(InputAction.CallbackContext ctx) {
        if (!ctx.performed) return;
        actionManager.ReceiveCombatInput(CombatInput.Jump);
        
    }
    #endregion

    #region ***---Attack---
    public void OnLightAttack(InputAction.CallbackContext ctx) {
        if (!ctx.performed) return;
        actionManager.ReceiveCombatInput(CombatInput.LightAttack);
    }
    
    public void OnHeavyAttack(InputAction.CallbackContext ctx) {
        if (!ctx.performed) return;
        actionManager.ReceiveCombatInput(CombatInput.HeavyAttack);
    }
    #endregion

    #region ***---Item---
    public void OnUseItem(InputAction.CallbackContext ctx) {
        if (ctx.performed && SelectedItem != null) SelectedItem.UseItem(this);
    }
    
    public void OnNextItem(InputAction.CallbackContext ctx) {
        if (!ctx.performed) return;
        if (itemInventory.Count != 0 && _selectedItemIndex == itemInventory.Count - 1) return;   
        _selectedItemIndex++;
        
        SelectedItem = itemInventory[_selectedItemIndex];
    }

    public void OnPreviousItem(InputAction.CallbackContext ctx) {
        if (!ctx.performed) return;
        if (itemInventory.Count != 0 && _selectedItemIndex == 0) return;
        _selectedItemIndex--;
        
        SelectedItem = itemInventory[_selectedItemIndex];
    }
    #endregion

    #region ***---Menu---
    public void OnPause(InputAction.CallbackContext ctx) {
    }
    
    public void OnUnpause(InputAction.CallbackContext ctx) {
    }
    #endregion
    #endregion
    
    private void Update() {
        if (!movementEnabled) return;
        
        entityMovement.MoveInDirection(Time.deltaTime, moveVector);
        CheckIfFlipObject();
        
        if (!isInAction) {
            animator.Play(moveVector.magnitude != 0 ? "Walk" : "Idle");
        }
    }
    
    public void ThrowItem(ThrowableItem throwableItem) {
        ThrowableObjectFactory.CreateGameObject(throwableItem, projectileSpawnPos.position, this);
    }
    
    public void CheckIfFlipObject() {
        if (moveVector.x < 0) _transform.localScale = _transform.localScale.With(x: -1);
        else if (moveVector.x > 0) _transform.localScale = _transform.localScale.With(x: 1);
    }
    
    private void OnDeath() {
        movementEnabled = false;
    }
}