using System;
using System.Collections.Generic;
using Dialogue;
using Items;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;
using Utils.Entity;
using Utils.EventBus;

public class PlayerScript : Damageable {
    [SerializeField] private Material testPlayer2Material;
    [SerializeField] private Transform projectileSpawnPosition;
    [SerializeField] protected Hitbox hitbox;
    
    [HideInInspector] public Vector2 moveVector;
    [HideInInspector] public bool movementEnabled = true;
    [HideInInspector] public bool inAction;
    
    public List<Item> itemInventory;

    private const string LoremIpsum =
        "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras consectetur ligula et quam eleifend, " +
        "ac sagittis nunc posuere. Quisque et porta lacus, id sollicitudin eros. Integer interdum ex eros, ac sagittis " +
        "nisi convallis lacinia. Phasellus eros sem, tempus eu orci non, lobortis iaculis sapien. Aenean sapien neque, " +
        "suscipit vitae ullamcorper eu, congue eget neque. Nullam gravida imperdiet semper. Sed bibendum, lacus egestas " +
        "pretium auctor, ligula erat condimentum massa, vel ullamcorper lacus massa ac orci. Vestibulum sodales nulla sit " +
        "amet lectus euismod, a ullamcorper mauris pharetra. Proin feugiat quam vitae augue sollicitudin commodo. Suspendisse " +
        "tristique fermentum nisi, quis interdum arcu eleifend eu. Etiam quis purus nec justo malesuada mollis id vel purus. " +
        "Curabitur iaculis ipsum quam, ac iaculis massa suscipit eget. Pellentesque euismod eros eu viverra scelerisque. Nunc " +
        "accumsan molestie faucibus.";
    
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
        
        if (!inAction) {
            animator.Play(moveVector.magnitude != 0 ? "Walk" : "Idle");
        }
        
        if (Keyboard.current.iKey.wasPressedThisFrame) {
            if (DialogueUIManager.DialogIsPlaying) {
                EventBus<DialogueEvent>.Raise(new SkipEvent());
            }
            else if (DialogueUIManager.InDialog) {
                EventBus<DialogueEvent>.Raise(new EndEvent());
            }
            else {
                EventBus<DialogueEvent>.Raise(new TextEvent("PersonI", LoremIpsum));
            }
        }
        
        if (Keyboard.current.oKey.wasPressedThisFrame) {
            if (DialogueUIManager.DialogIsPlaying) {
                EventBus<DialogueEvent>.Raise(new SkipEvent());
            }
            else if (DialogueUIManager.InDialog) {
                EventBus<DialogueEvent>.Raise(new EndEvent());
            }
            else {
                EventBus<DialogueEvent>.Raise(new TextEvent("PersonO", LoremIpsum));
            }
        }
    }
    
    public void ThrowItem(ThrowableItem throwableItem) {
        ThrowableObjectFactory.CreateGameObject(throwableItem, projectileSpawnPosition.position, this);
    }
    
    public void CheckIfFlipObject() {
        if (moveVector.x < 0) _transform.localScale = _transform.localScale.With(x: -1);
        else if (moveVector.x > 0) _transform.localScale = _transform.localScale.With(x: 1);
    }
    
    private void OnDeath() {
        movementEnabled = false;
    }
}