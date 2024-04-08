using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class DirectionalInputManager : MonoBehaviour {
    
    public static Direction DirectionFormVector2(Vector2 vector) {
        var angle = math.atan2(vector.y, vector.x);
        var octant = (int)(8 * angle / (2*math.PI) + 8) % 8;
        return Directions[octant];
    }

    private static readonly Dictionary<int,Direction> Directions = new() {
        { 0, Direction.Forward},
        { 1, Direction.UpDiagonal},
        { 2, Direction.Up},
        { 3, Direction.UpDiagonal},
        { 4, Direction.Forward},
        { 5, Direction.DownDiagonal},
        { 6, Direction.Down},
        { 7, Direction.DownDiagonal}
    };
    
    public enum Direction {
        Forward,
        UpDiagonal, Up,
        Down, DownDiagonal,
        None
    };
}