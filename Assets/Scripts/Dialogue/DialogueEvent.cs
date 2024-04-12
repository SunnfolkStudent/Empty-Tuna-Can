using System;
using Utils.EventBus;

namespace Dialogue {
    public class DialogueEvent : IEvent {
        public readonly IDialogEvent Event;

        public DialogueEvent(IDialogEvent @event) {
            Event = @event;
        }
    }

    public interface IDialogEvent {
    }
    
    [Serializable] public struct TextEvent : IDialogEvent {
        public string name;
        public string text;
        public float textSpeed;
        
        public TextEvent(string name, string text, float textSpeed = 15) {
            this.name = name;
            this.text = text;
            this.textSpeed = textSpeed;
        }
    }
    
    public struct SkipEvent : IDialogEvent {
    }
    
    public struct EndEvent : IDialogEvent {
    }
}