using UnityEngine;

namespace Entities {
    public class EntityMovement : MonoBehaviour {
        public float verticalSpeed = 1;
        public float horizontalSpeed = 2;
    
        private Transform _transform;
    
        private void Awake() {
            _transform = transform;
        }
    
        public void MoveInDirection(Vector2 direction) {
            _transform.position += new Vector3(direction.x * horizontalSpeed, direction.y * verticalSpeed, 0) * Time.deltaTime;
        }
    }
}