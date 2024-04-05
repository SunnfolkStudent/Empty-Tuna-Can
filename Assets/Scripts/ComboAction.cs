using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "ComboAction", menuName = "Combo/new ComboAction")]
public class ComboAction : ScriptableObject {
    public AnimationClip animation;
    public bool canMoveDuring;
    public string[] keyCombo;

    public void Execute() {}
}