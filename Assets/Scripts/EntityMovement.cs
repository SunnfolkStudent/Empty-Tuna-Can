using UnityEngine;

public class EntityMovement : MonoBehaviour {
    public float verticalSpeed = 1;
    public float horizontalSpeed = 2;

    private const float VerticalDodgeSpeed = 10;

    private Rigidbody2D _rigidbody;
    private Transform _transform;

    private void Awake() {
        _transform = transform;
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void MoveInDirection(float deltaTime, Vector2 direction) {
        _transform.position += new Vector3(direction.x * horizontalSpeed, direction.y * verticalSpeed, 0) * deltaTime;

        // var newPos = _transform.position + new Vector3(direction.x * horizontalSpeed, direction.y * verticalSpeed, 0) * deltaTime;
        // MoveToPosition(newPos);
    }

    // private void MoveToPosition(Vector3 targetPosition) {
    //     var c = Physics2D.OverlapPoint(targetPosition, LayerMask.GetMask("WalkableArea"));
    //     if (c != null) {
    //         _transform.position = targetPosition;
    //     }
    // }
    
    private void DodgeUp() {
        _rigidbody.velocity = Vector2.up * VerticalDodgeSpeed;
    }
    
    private void DodgeDown() {
        _rigidbody.velocity = Vector2.down * VerticalDodgeSpeed;
    }
}