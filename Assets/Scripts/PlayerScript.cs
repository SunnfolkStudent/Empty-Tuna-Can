using Items;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;
using Utils.Entity;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerScript : Damageable {
    [HideInInspector] public Vector2 moveVector;
    [HideInInspector] public bool movementEnabled = true;
    [HideInInspector] public bool inAction;
    
    [Header("References")]
    [SerializeField] protected Hitbox hitbox;
    [SerializeField] private Animator animator;
    [SerializeField] private ActionManager actionManager;
    [SerializeField] private EntityMovement entityMovement;
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private Transform projectileSpawnPosition;
    
    public Inventory inventory;
    private Transform _transform;
    
    private const float VerticalDodgeSpeed = 10;
    
    private CombatInput _currentDirection;

    [Header("Players")] 
    [SerializeField] private Material[] playerColors;
    
    private static Conversation conversation;
    
    private Rigidbody2D _rigidbody;
    private static readonly int Speed = Animator.StringToHash("Speed");

    private void Awake() {
        _transform = transform;
        entityMovement = GetComponent<EntityMovement>();
        _rigidbody = GetComponent<Rigidbody2D>();
        EntityManager.PlayersTransforms.Add(_transform);
    }
    
    private void Start() {
        OnDying += OnDeath;
        
        inventory.Initialize();
        PlayerUIFactory.CreatePlayerUI(this);
        
        teamNumber = EntityManager.PlayersTransforms.Count;
        
        transform.name = "Player" + teamNumber;
        playerSprite.material = playerColors[teamNumber - 1];
        
        hitbox.teamNumber = 1;
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

        if (conversation != null) {
            conversation.Next();
            return;
        }
        
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
        animator.SetFloat(Speed, moveVector.magnitude);
        
        if (!movementEnabled) return;
        entityMovement.MoveInDirection(moveVector);
        CheckIfFlipObject();
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
    
    private void DodgeUp() {
        _rigidbody.velocity = Vector2.up * VerticalDodgeSpeed;
    }
    
    private void DodgeDown() {
        _rigidbody.velocity = Vector2.down * VerticalDodgeSpeed;
    }
    
    public static void StartConversation(Conversation newConversation) {
        conversation = newConversation;
        conversation.Next();
    }

    protected override void Stagger() {
        animator.Play("Stagger");
        movementEnabled = false;
        inAction = true;
    }
}