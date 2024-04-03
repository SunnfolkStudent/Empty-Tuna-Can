using UnityEngine.Events;
using Utils.Singleton;

public class PauseManager : PersistentSingleton<PauseManager> {
    public event UnityAction PauseAction;
    public event UnityAction UnpauseAction;

    public void Pause() {
        PauseAction?.Invoke();
    }
    
    public void Unpause() {
        UnpauseAction?.Invoke();
    }
}