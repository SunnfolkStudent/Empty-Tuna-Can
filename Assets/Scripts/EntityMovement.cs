using UnityEngine;

public class EntityMovement : MonoBehaviour {
    public float verticalSpeed = 1;
    public float horizontalSpeed = 2;
    
    public void MoveInDirection(float deltaTime, Vector2 direction) {
        var transform1 = transform;
        
        var newPos = transform1.position + new Vector3(direction.x * horizontalSpeed, direction.y * verticalSpeed, 0) * deltaTime;
        MoveToPosition(newPos);
    }

    private void MoveToPosition(Vector3 targetPosition) {
        // var c = Physics2D.OverlapPoint(targetPosition, LayerMask.GetMask("WalkableArea"));
        // if (c != null) {
            transform.position = targetPosition;
        // }
    }
}