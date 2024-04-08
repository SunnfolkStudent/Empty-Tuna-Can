using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "CombatAction", menuName = "Combat/new CombatAction")]
public class CombatAction : ScriptableObject {
    [Header("Execution")]
    public CombatInput[] combatInputs;
    
    [Header("Animation")]
    public AnimationClip animation;
}