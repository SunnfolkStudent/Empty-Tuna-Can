using UnityEngine;
using UnityEngine.InputSystem;

public class TestMove : MonoBehaviour {
    [SerializeField] private float speed = 0.5f;
    [SerializeField] private float jumpSpeed = 1.0f;
    
    private Vector2 _moveVector;
    private Vector3 _gravityVelocity;
    
    private SpriteRenderer _spriteRenderer;
    private CharacterController _characterController;
    private Animator _animator;
    
    private void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }
    
    public void OnMove(InputAction.CallbackContext ctx) => _moveVector = ctx.ReadValue<Vector2>();
    public void OnJump(InputAction.CallbackContext ctx) => Jump();
    public void OnPause(InputAction.CallbackContext ctx) {}
    public void OnUnpause(InputAction.CallbackContext ctx) {}
    
    private void FixedUpdate() {
        var move = new Vector3(_moveVector.x, 0, _moveVector.y) * speed;
        move += _gravityVelocity * Time.deltaTime;
        
        _characterController.Move(move);

        if (_moveVector.magnitude != 0) {
            _animator.Play("Walk");
        }
        else {
            _animator.Play("Idle");
        }
        
        if (_moveVector.x < 0) _spriteRenderer.flipX = true;
        else if (_moveVector.x > 0) _spriteRenderer.flipX = false;
    }
    
    private void Update() {
        _gravityVelocity += Physics.gravity * Time.deltaTime;
        if (_characterController.isGrounded) {
            _gravityVelocity = Physics.gravity.normalized * 2;
        }
    }
    
    private void Jump() {
        if (_characterController.isGrounded) {
            _characterController.Move(new Vector3(0, jumpSpeed, 0));
        }
    }
}