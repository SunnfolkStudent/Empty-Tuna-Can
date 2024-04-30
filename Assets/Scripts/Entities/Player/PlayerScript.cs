using System.Collections.Generic;
using System.Linq;
using Dialogue;
using Items;
using ModeManagers;
using StateMachineBehaviourScripts;
using UnityEngine;
using UnityEngine.InputSystem;
using Upgrades;
using Utils;
using Utils.EventBus;

namespace Entities.Player {
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
        [HideInInspector] public Transform transform1;
        
        private const float VerticalDodgeSpeed = 10;
        
        private CombatInput _currentDirection;
        
        [Header("Players")] 
        private int playerNumber;
        [SerializeField] private Material[] playerColors;
        public int wins;
        
        [Header("Upgrades")]
        public List<Upgrade> allUpgrades = new ();
        
        private static Conversation conversation;
        
        public static bool Paused;
        
        private Rigidbody2D _rigidbody;
        public bool dead;
        
        private static readonly int SpeedAnimatorParameter = Animator.StringToHash("Speed");
        private static readonly int IsDeadAnimatorParameter = Animator.StringToHash("IsDead");
        
        private void Awake() {
            transform1 = transform;
            entityMovement = GetComponent<EntityMovement>();
            _rigidbody = GetComponent<Rigidbody2D>();
            PlayerManager.RegisterPlayer(this);
            
            foreach (var stateBehaviour in animator.GetBehaviours<Attack>()) {
                stateBehaviour.playerScript = this;
            }
        }

        private void OnDisable() {
            PlayerManager.DeregisterPlayer(this);
        }

        private void Start() {
            OnDying += OnDeath;
            
            inventory.Initialize();
            PlayerUIFactory.CreatePlayerUI(this);
            
            playerNumber = PlayerManager.AllPlayers.Count;
            transform.name = "Player" + playerNumber;
            playerSprite.material = playerColors[playerNumber - 1];
            
            teamNumber = PlayerManager.FriendlyFire ? 0 : 1;
            hitbox.teamNumber = teamNumber;
            
            transform.position = PlayerManager.GetAvailableSpawnPosition();
        }
        
        #region ---OnInputAction---
        #region ***---Movement---
        public void OnMove(InputAction.CallbackContext ctx) {
            if (Paused) return;
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
            if (Paused) return;
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
            if (Paused) return;
            if (!ctx.performed) return;
            actionManager.ReceiveCombatInput(CombatInput.LightAttack);
        }
        
        public void OnHeavyAttack(InputAction.CallbackContext ctx) {
            if (Paused) return;
            if (!ctx.performed) return;
            actionManager.ReceiveCombatInput(CombatInput.HeavyAttack);
        }
        #endregion
        
        #region ***---Item---
        public void OnUseItem(InputAction.CallbackContext ctx) {
            if (Paused) return;
            if (!ctx.performed || inventory.selectedItem.Value.item == null) return;
            inventory.UseSelectedItem(this);
        }
        
        public void OnNextItem(InputAction.CallbackContext ctx) {
            if (Paused) return;
            if (!ctx.performed) return;
            inventory.NextItem();
        }
        
        public void OnPreviousItem(InputAction.CallbackContext ctx) {
            if (Paused) return;
            if (!ctx.performed) return;
            inventory.PreviousItem();
        }
        #endregion
        
        #region ***---Menu---
        public void OnPause(InputAction.CallbackContext ctx) {
            if (!ctx.performed) return;
            EventBus<PauseMenuEvent>.Raise(new PauseMenuEvent(!Paused));
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
            if (moveVector.x < 0) transform1.localScale = transform1.localScale.With(x: -1);
            else if (moveVector.x > 0) transform1.localScale = transform1.localScale.With(x: 1);
        }
        
        private void OnDeath() {
            if (dead) return;
            animator.SetBool(IsDeadAnimatorParameter, true);
            EventBus<GameModeEvent>.Raise(new GameModeEvent(new PlayerDeathEvent(this)));
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
            if (dead) return;
            animator.Play("Stagger");
            movementEnabled = false;
        }
        
        private DamageInstance GetDamageInstance(AttackAction attackAction) {
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