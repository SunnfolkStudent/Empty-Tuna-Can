using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "ComboAction", menuName = "Combat/new ComboAction")]
public class ComboAction : ScriptableObject {
    [Header("Execution")]
    public CombatInput[] combatInputs;
    
    [Header("Animation")]
    public AnimationClip animation;
}