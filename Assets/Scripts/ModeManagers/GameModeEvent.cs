using Entities.Player;
using Utils.EventBus;

namespace ModeManagers {
    public class GameModeEvent : IEvent {
        public readonly IGameModeEvent Event;

        public GameModeEvent(IGameModeEvent @event) {
            Event = @event;
        }
    }

    public interface IGameModeEvent {
    }
    
    public class CutsceneOverEvent : IGameModeEvent {
    }

    public class PlayerDeathEvent : IGameModeEvent {
        public readonly PlayerScript PlayerScript;

        public PlayerDeathEvent(PlayerScript playerScript) {
            PlayerScript = playerScript;
        }
    }

    public class WaveOver : IGameModeEvent {
    }
}