using Player;
using UnityEngine;

namespace Interact {
    public abstract class InteractBase : MonoBehaviour {
        private bool hasInteracted;
    
        private void OnTriggerEnter2D(Collider2D other) {
            if (!other.TryGetComponent(out PlayerScript player) || hasInteracted) return;
            hasInteracted = true;
            InteractionLogic(player);
        }
    
        protected virtual void InteractionLogic(PlayerScript player) {
        }
    }
}