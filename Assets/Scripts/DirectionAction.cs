using UnityEngine;

[CreateAssetMenu(fileName = "DirectionAction", menuName = "Combat/new DirectionAction")]
public class DirectionAction : ScriptableObject {
    public DirectionalInputManager.Direction[] directionInputs;
    public CombatInput combatInput;
}