using Items;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public static class ThrowableObjectFactory {
    public static void CreateGameObject(ThrowableItem throwableItem, Vector3 position, Vector2 facingDirection, GameObject owner) {
        var o = Object.Instantiate(throwableItem.itemPrefab, position, quaternion.identity);
        
        var rb = o.GetComponent<Rigidbody>();
        rb.velocity = facingDirection * throwableItem.velocity;

        var s = o.GetOrAddComponent<ThrowableObject>();
        s.damage = throwableItem.damage;
        s.owner = owner;
    }
}