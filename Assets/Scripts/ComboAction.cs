using UnityEngine;

[CreateAssetMenu(fileName = "ComboAction", menuName = "new ComboAction")]
public class ComboAction : ScriptableObject {
    public AnimationClip animation;
    public string[] keyCombo;

    public void Execute() {}
}