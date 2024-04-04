using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

public class TestMove : MonoBehaviour {
    [SerializeField] private float speed = 0.5f;
    [SerializeField] private float jumpSpeed = 1.0f;
    
    private Vector2 _moveVector;
    private Vector3 _gravityVelocity;
    
    private SpriteRenderer _spriteRenderer;
    private CharacterController _characterController;
    private Animator _animator;
    private ActionManager _actionManager;

    public bool movementEnabled = true;
    
    private void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _actionManager = GetComponent<ActionManager>();
    }
    
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

    public void OnLightAttack(InputAction.CallbackContext ctx) {
        if (ctx.performed) _actionManager.ReceiveAction(ctx);
    }
    
    public void OnHeavyAttack(InputAction.CallbackContext ctx) {
        if (ctx.performed) _actionManager.ReceiveAction(ctx);
    }
    
    public void OnPause(InputAction.CallbackContext ctx) {}
    public void OnUnpause(InputAction.CallbackContext ctx) {}
    
    private void FixedUpdate() {
        if (!movementEnabled) return;
        
        var move = new Vector3(_moveVector.x, 0, _moveVector.y) * speed;
        move += _gravityVelocity * Time.deltaTime;
        
        _characterController.Move(move);
        
        if (_moveVector.x < 0) _spriteRenderer.flipX = true;
        else if (_moveVector.x > 0) _spriteRenderer.flipX = false;
    }
    
    private void Update() {
        _gravityVelocity += Physics.gravity * Time.deltaTime;
        if (_characterController.isGrounded) {
            _gravityVelocity = Physics.gravity.normalized * 2;
        }
        
        if (_animator.GetCurrentAnimationClip().name is "Idle") {
            movementEnabled = true;
        }
        
        if (!movementEnabled) return;
        _animator.Play(_moveVector.magnitude != 0 ? "Walk" : "Idle");
    }
}