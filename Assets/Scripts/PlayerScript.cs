using Items;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;
using Utils.Entity;

public class PlayerScript : Damageable {
    [SerializeField] private Material testPlayer2Material;
    [SerializeField] private Transform projectileSpawnPosition;
    [SerializeField] protected Hitbox hitbox;
    
    [HideInInspector] public Vector2 moveVector;
    [HideInInspector] public bool movementEnabled = true;
    [HideInInspector] public bool inAction;
    
    
    [Header("Script References")]
    [SerializeField] private Animator animator;
    [SerializeField] private ActionManager actionManager;
    [SerializeField] private EntityMovement entityMovement;
    public Inventory inventory;
    private Transform _transform;
    
    private CombatInput _currentDirection;
    
    private void Awake() {
        _transform = transform;
        entityMovement = GetComponent<EntityMovement>();
    }
    
    private void Start() {
        OnDying += OnDeath;
        
        inventory.Initialize();
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
        if (!ctx.performed || inventory.selectedItem.Value.item == null) return;
        inventory.UseSelectedItem(this);
    }
    
    public void OnNextItem(InputAction.CallbackContext ctx) {
        if (!ctx.performed) return;
        inventory.NextItem();
    }

    public void OnPreviousItem(InputAction.CallbackContext ctx) {
        if (!ctx.performed) return;
        inventory.PreviousItem();
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