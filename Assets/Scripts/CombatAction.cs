using System;
using Player;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "CombatAction", menuName = "Combat/new CombatAction")]
public class CombatAction : ScriptableObject {
    [Header("Execution")]
    public CombatInput[] combatInputs;
    
    // [Header("Animation")]
    // public AnimationClip animation;
    
    [Header("CombatOutput")]
    public CombatOutput combatOutput;
    public bool canMoveDuring;
}