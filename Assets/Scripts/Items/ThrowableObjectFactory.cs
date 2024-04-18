using Player;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

namespace Items {
    public static class ThrowableObjectFactory {
        public static void CreateGameObject(ThrowableItem throwableItem, Vector3 position, PlayerScript owner) {
            var o = Object.Instantiate(throwableItem.itemPrefab, position, quaternion.identity);
            o.transform.localScale = owner.transform.localScale;

            var s = o.GetOrAddComponent<ThrowableObject>();
            s.damage = throwableItem.damage;
            s.teamNumber = owner.teamNumber;
        }
    }
}