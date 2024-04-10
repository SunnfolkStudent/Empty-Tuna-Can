using Utils.EventBus;

namespace Dialogue {
    public class DialogueEvent : IEvent {
    }

    public class TextEvent : DialogueEvent {
        public readonly string Name;
        public readonly string Text;

        public TextEvent(string name, string text) {
            Name = name;
            Text = text;
        }
    }
 
    public class SkipEvent : DialogueEvent {
    }

    public class EndEvent : DialogueEvent {
    }
}