using Unity.Mathematics;
using UnityEngine;

public static class ThrowableObject {
    public static GameObject CreateGameObject(GameObject gameObject, Vector3 position, float velocity, Vector2 facingDirection) {
        var o = Object.Instantiate(gameObject, position, quaternion.identity);
        
        var rb = o.GetComponent<Rigidbody>();
        rb.velocity = facingDirection * velocity;
        
        return o;
    }
}