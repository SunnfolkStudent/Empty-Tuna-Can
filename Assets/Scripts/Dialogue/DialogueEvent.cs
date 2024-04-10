using Utils.EventBus;

namespace Dialogue {
    public class DialogueEvent : IEvent {
    }
    
    public class TextEvent : DialogueEvent {
        public readonly string Name;
        public readonly string Text;
        public readonly float TextSpeed;

        public TextEvent(string name, string text, float textSpeed = 0) {
            Name = name;
            Text = text;
            TextSpeed = textSpeed;
        }
    }
    
    public class SkipEvent : DialogueEvent {
    }
    
    public class EndEvent : DialogueEvent {
    }
}