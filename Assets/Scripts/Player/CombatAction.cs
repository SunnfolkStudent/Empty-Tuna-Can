using System;
using UnityEngine;

namespace Player {
    [Serializable]
    [CreateAssetMenu(fileName = "CombatAction", menuName = "Combat/new CombatAction")]
    public class CombatAction : ScriptableObject {
        [Header("Execution")]
        public CombatInput[] combatInputs;
    
        [Header("CombatOutput")]
        public CombatOutput combatOutput;
    }
}