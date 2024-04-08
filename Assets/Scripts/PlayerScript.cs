using System;
using System.Collections.Generic;
using Items;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

public class PlayerScript : Entity {
    [SerializeField] private float jumpSpeed = 1.0f;
    
    [SerializeField] private Transform projectileSpawnPos;
    
    public List<Item> item;
    
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
    
    public int selectedItemIndex;
    #endregion
    
    private CombatInput _currentDirection;
    private Animator _animator;
    private ActionManager _actionManager;
    
    private new void Awake() {
        base.Awake();
        _animator = GetComponent<Animator>();
        _actionManager = GetComponent<ActionManager>();
    }
    
    private void Start() {
        item.Add(ScrubUtils.GetAllScrubsInResourceFolder<Item>("Items").GetByName("TunaCan"));
        item.Add(ScrubUtils.GetAllScrubsInResourceFolder<Item>("Items").GetByName("EmptyTunaCan"));
        
        SelectedItem = item[0];
        OnDying += OnDeath;
        
        PlayerUIFactory.CreatePlayerUI(this);
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
            _actionManager.ReceiveCombatInput(_currentDirection);
        }
    }

    public void OnJump(InputAction.CallbackContext ctx) {
        if (!ctx.performed) return;
        
        if (CharacterController.isGrounded) {
            CharacterController.Move(new Vector3(0, jumpSpeed, 0));
        }
    }
    #endregion

    #region ***---Attack---
    public void OnLightAttack(InputAction.CallbackContext ctx) {
        if (ctx.performed) {
            _actionManager.ReceiveCombatInput(CombatInput.LightAttack);
        }
    }
    
    public void OnHeavyAttack(InputAction.CallbackContext ctx) {
        if (ctx.performed) {
            _actionManager.ReceiveCombatInput(CombatInput.HeavyAttack);
        }
    }
    #endregion

    #region ***---Item---
    public void OnUseItem(InputAction.CallbackContext ctx) {
        if (ctx.performed && SelectedItem != null) SelectedItem.UseItem(this);
    }
    
    public void OnNextItem(InputAction.CallbackContext ctx) {
        if (!ctx.performed) return;
        if (item.Count != 0 && selectedItemIndex == item.Count - 1) return;   
        selectedItemIndex++;
        
        SelectedItem = item[selectedItemIndex];
    }

    public void OnPreviousItem(InputAction.CallbackContext ctx) {
        if (!ctx.performed) return;
        if (item.Count != 0 && selectedItemIndex == 0) return;
        selectedItemIndex--;
        
        SelectedItem = item[selectedItemIndex];
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
        GravityVelocity += Physics.gravity * Time.deltaTime;
        if (CharacterController.isGrounded) {
            GravityVelocity = Physics.gravity.normalized * 2;
        }
        
        if (!movementEnabled) return;
        _animator.Play(moveVector.magnitude != 0 ? "Walk" : "Idle");
        
        var transform1 = transform;
        if (moveVector.x < 0) transform1.localScale = transform1.localScale.With(x: -1);
        else if (moveVector.x > 0) transform1.localScale = transform1.localScale.With(x: 1);
    }
    
    public void ThrowItem(ThrowableItem throwableItem) {
        var facingDirection = new Vector2(transform.localScale.x,0);
        ThrowableObjectFactory.CreateGameObject(throwableItem, projectileSpawnPos.position, facingDirection, gameObject);
    }
    
    private void OnDeath() {
        movementEnabled = false;
    }
}