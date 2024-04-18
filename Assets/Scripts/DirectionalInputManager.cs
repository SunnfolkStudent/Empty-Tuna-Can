using System.Collections.Generic;
using Player;
using Unity.Mathematics;
using UnityEngine;

public class DirectionalInputManager : MonoBehaviour {
    
    public static CombatInput DirectionFormVector2(Vector2 vector) {
        var angle = math.atan2(vector.y, vector.x);
        var octant = (int)(8 * angle / (2*math.PI) + 8) % 8;
        return Directions[octant];
    }

    private static readonly Dictionary<int, CombatInput> Directions = new() {
        { 0, CombatInput.Forward},
        { 1, CombatInput.UpDiagonal},
        { 2, CombatInput.Up},
        { 3, CombatInput.UpDiagonal},
        { 4, CombatInput.Forward},
        { 5, CombatInput.DownDiagonal},
        { 6, CombatInput.Down},
        { 7, CombatInput.DownDiagonal}
    };
}