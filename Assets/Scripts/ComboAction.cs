using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "ComboAction", menuName = "Combo/new ComboAction")]
public class ComboAction : ScriptableObject {
    [Header("Prerequisites")]
    public InputDirections[] inputDirections;
    
    public ComboAction previousActionInChain;
    public bool grounded = true;
    
    [Header("Execution")]
    public string[] keyCombo;
    
    [Header("Animation")]
    public AnimationClip animation;
}

public enum InputDirections { Up, Down, Left, Right }