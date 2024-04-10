using UnityEngine;
using UnityEngine.InputSystem;

public class TestCameraControls : MonoBehaviour {
    [SerializeField] private float cameraSpeed = 5;
    private void Update() {
        if (Keyboard.current.leftArrowKey.isPressed) {
            transform.position += new Vector3(-cameraSpeed, 0, 0) * Time.deltaTime;
        }
        
        if (Keyboard.current.rightArrowKey.isPressed) {
            transform.position += new Vector3(cameraSpeed, 0, 0) * Time.deltaTime;
        }
    }
}