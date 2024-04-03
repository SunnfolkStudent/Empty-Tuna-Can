using UnityEngine;

namespace Utils {
    public class RotateTowardsMouse : MonoBehaviour {
        [SerializeField] private GameObject rotatedObjects; 
        
        private void Update() {
            var mousePos = Input.mousePosition;
            var mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, transform.position.z - Camera.main.transform.position.z));
            var direction = mouseWorldPos - transform.position;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            var rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            rotatedObjects.transform.rotation = rotation;
        }
    }
}