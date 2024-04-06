using System;
using System.Collections.Generic;
using Items;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;
using Utils.Entity;

public class PlayerScript : Damageable {
    [SerializeField] private float speed = 0.5f;
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
    
    private Vector2 _moveVector;
    private Vector3 _gravityVelocity;
    
    private CharacterController _characterController;
    private Animator _animator;
    private ActionManager _actionManager;
    
    public bool movementEnabled = true;
    
    private void Awake() {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _actionManager = GetComponent<ActionManager>();
    }
    
    private void Start() {
        item.Add(ScrubUtils.GetAllScrubsInResourceFolder<Item>("Items").GetByName("TunaCan"));
        
        SelectedItem = item[0];
        OnDying += OnDeath;
        
        PlayerUIFactory.CreatePlayerUI(this);
    }
    
    #region ---OnInputAction---
    #region ***---Movement---
    public void OnMove(InputAction.CallbackContext ctx) {
        _moveVector = ctx.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext ctx) {
        if (!ctx.performed) return;
        
        _actionManager.ReceiveAction(ctx);

        if (_characterController.isGrounded) {
            _characterController.Move(new Vector3(0, jumpSpeed, 0));
        }
    }
    #endregion

    #region ***---Attack---
    public void OnLightAttack(InputAction.CallbackContext ctx) {
        if (ctx.performed) _actionManager.ReceiveAction(ctx);
    }
    
    public void OnHeavyAttack(InputAction.CallbackContext ctx) {
        if (ctx.performed) _actionManager.ReceiveAction(ctx);
    }
    #endregion

    #region ***---Item---
    public void OnUseItem(InputAction.CallbackContext ctx) {
        if (ctx.performed && SelectedItem != null) SelectedItem.UseItem(this);
    }
    
    public void OnNextItem(InputAction.CallbackContext ctx) {
        if (!ctx.performed) return;
        if (selectedItemIndex == item.Count - 1) return;   
        selectedItemIndex++;
        
        SelectedItem = item[selectedItemIndex];
    }

    public void OnPreviousItem(InputAction.CallbackContext ctx) {
        if (!ctx.performed) return;
        if (selectedItemIndex == 0) return;
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
    
    private void FixedUpdate() {
        if (!movementEnabled) return;
        
        var move = new Vector3(_moveVector.x, 0, _moveVector.y) * speed;
        move += _gravityVelocity * Time.deltaTime;
        
        _characterController.Move(move);
    }
    
    private void Update() {
        _gravityVelocity += Physics.gravity * Time.deltaTime;
        if (_characterController.isGrounded) {
            _gravityVelocity = Physics.gravity.normalized * 2;
        }
        
        if (!movementEnabled) return;
        _animator.Play(_moveVector.magnitude != 0 ? "Walk" : "Idle");
        
        var transform1 = transform;
        if (_moveVector.x < 0) transform1.localScale = transform1.localScale.With(x: -1);
        else if (_moveVector.x > 0) transform1.localScale = transform1.localScale.With(x: 1);
    }
    
    public void ThrowItem(ThrowableItem throwableItem) {
        var facingDirection = new Vector2(transform.localScale.x,0);
        ThrowableObjectFactory.CreateGameObject(throwableItem, projectileSpawnPos.position, facingDirection, gameObject);
    }

    private void OnDeath() {
        movementEnabled = false;
    }
}