using Player;
using UnityEngine;

namespace Dialogue {
    public class StartConversation : InteractBase {
        [SerializeField] private Conversation conversation;
        
        protected override void InteractionLogic(PlayerScript player) {
            PlayerScript.StartConversation(conversation);
        }
    }
}