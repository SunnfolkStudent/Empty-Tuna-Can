using UnityEngine;
using Utils.EventBus;

namespace Dialogue {
    [CreateAssetMenu(fileName = "Conversation", menuName = "new Conversation")]
    public class Conversation : ScriptableObject {
        public TextEvent[] textEvents;
        private int conversationIndex;
    
        public void Next() {
            if (DialogueUIManager.DialogIsPlaying) {
                EventBus<DialogueEvent>.Raise(new DialogueEvent(new SkipEvent()));
            }
            else if (conversationIndex >= textEvents.Length) {
                EventBus<DialogueEvent>.Raise(new DialogueEvent(new EndEvent()));
                conversationIndex = 0;
            }
            else {
                EventBus<DialogueEvent>.Raise(new DialogueEvent(textEvents[conversationIndex]));
                conversationIndex++;
            }
        }
    }
}