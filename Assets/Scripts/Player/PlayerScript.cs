using System.Collections.Generic;
using System.Linq;
using Items;
using StateMachineBehaviourScripts;
using UnityEngine;
using UnityEngine.InputSystem;
using Upgrades;
using Utils;
using Utils.Entity;

namespace Player {
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerScript : Damageable {
        [Header("Movement")]
        public Vector2 moveVector;
        public bool movementEnabled = true;
    
        [Header("References")]
        public Hitbox hitbox;
        [SerializeField] public Animator animator;
        [SerializeField] private ActionManager actionManager;
        public EntityMovement entityMovement;
        [SerializeField] private SpriteRenderer playerSprite;
        [SerializeField] private Transform projectileSpawnPosition;
        public PlayerInput input;
    
        public Inventory inventory;
        private Transform _transform;
    
        private const float VerticalDodgeSpeed = 10;
    
        private CombatInput _currentDirection;

        public static bool FriendlyFire = true;

        [Header("Players")] 
        private int playerNumber;
        [SerializeField] private Material[] playerColors;
    
        [Header("Upgrades")]
        public List<Upgrade> allUpgrades = new ();
    
        private static Conversation conversation;
    
        private Rigidbody2D _rigidbody;
        private static readonly int SpeedAnimatorParameter = Animator.StringToHash("Speed");

        private void Awake() {
            _transform = transform;
            entityMovement = GetComponent<EntityMovement>();
            _rigidbody = GetComponent<Rigidbody2D>();
            EntityManager.PlayersTransforms.Add(_transform);
        
            foreach (var stateBehaviour in animator.GetBehaviours<Attack>()) {
                stateBehaviour.playerScript = this;
            }
        }
    
        private void Start() {
            OnDying += OnDeath;
        
            inventory.Initialize();
            PlayerUIFactory.CreatePlayerUI(this);
        
            playerNumber = EntityManager.PlayersTransforms.Count;
            transform.name = "Player" + playerNumber;
            playerSprite.material = playerColors[playerNumber - 1];
        
            teamNumber = FriendlyFire ? 0 : 1;
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
            if (!movementEnabled) return;
            animator.SetFloat(SpeedAnimatorParameter, moveVector.magnitude);
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
        }

        public DamageInstance GetDamageInstance(AttackAction attackAction) {
            return animator.GetBehaviours<Attack>().ToList().First(behaviour => behaviour.attackAction == attackAction).damageInstance;
        }
    
        public void GetUpgrade(Upgrade upgrade) {
            switch (upgrade) {
                case CharacterUpgrade characterUpgrade:
                    characterUpgrade.GetUpgrade(this);
                    break;
                case AttackUpgrade attackUpgrade:
                    var damageInstance = GetDamageInstance(attackUpgrade.attackAction);
                    attackUpgrade.GetUpgrade(damageInstance);
                    break;
            }
        }
    }
}