using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils {
    public static class ExtensionMethods {
        /// <summary>
        /// Retrieves all immediate child GameObjects of a given Transform.
        /// </summary>
        /// <param name="parent">The parent Transform from which to get the immediate children.</param>
        /// <returns>An array of GameObjects that are the immediate children of the specified parent Transform.</returns>
        public static GameObject[] GetImmediateChildren(this Transform parent) {
            var childCount = parent.childCount;
            var children = new GameObject[childCount];
        
            for (var i = 0; i < childCount; i++) {
                children[i] = parent.GetChild(i).gameObject;
            }
        
            return children;
        }
        
        /// <summary>
        /// Converts a time in seconds to a formatted string representation.
        /// </summary>
        /// <param name="timeInSeconds">The time in seconds to convert.</param>
        /// <param name="format">The string format to use for the time representation.</param>
        /// <returns>A string representing the time in the specified format.</returns>
        public static string ToStringTimeFormat(this float timeInSeconds, string format) {
            return TimeSpan.FromSeconds(timeInSeconds).ToString(format);
        } 
        
        /// <summary>
        /// Returns a random element from the list or array.
        /// </summary>
        public static T GetRandom<T>(this IReadOnlyList<T> list) {
            var random = new System.Random();
            return list[random.Next(0, list.Count)];
        }
        
        /// <summary>
        /// Returns the current animation clip being played by the Animator
        /// </summary>
        public static AnimationClip GetCurrentAnimationClip(this Animator animator) {
            return animator.GetCurrentAnimatorClipInfo(0)[0].clip;
        }
        
        #region ---Vector3---
        /// <summary>
        /// Returns the same vector plus given x, y and/or z offset
        /// </summary>
        /// <param name="x">Add offset in the X-axis</param>
        /// <param name="y">Add offset in the Y-axis</param>
        /// <param name="z">Add offset in the Z-axis</param>
        public static Vector3 WithOffset(this Vector3 vector3, float x = 0, float y = 0, float z = 0) {
            return vector3 + new Vector3(x, y, z);
        }
        
        /// <summary>
        /// Returns the vector with values set
        /// </summary>
        /// <param name="x">Sets the value of the X-axis</param>
        /// <param name="y">Sets the value of the Y-axis</param>
        /// <param name="z">Sets the value of the Z-axis</param>
        public static Vector3 With(this Vector3 vector3, float? x = null, float? y = null, float? z = null) {
            if (x.HasValue)
                vector3.x = x.Value;
            if (y.HasValue)
                vector3.y = y.Value;
            if (z.HasValue)
                vector3.z = z.Value;
            return vector3;
        }
        #endregion
        
        #region ---Vector2---
        /// <summary>
        /// Returns the same vector plus given x and/or y offset
        /// </summary>
        /// <param name="x">Add offset in the X-axis</param>
        /// <param name="y">Add offset in the Y-axis</param>
        public static Vector2 WithOffset(this Vector2 vector2, float x = 0, float y = 0) {
            return vector2 + new Vector2(x, y);
        }
        
        /// <summary>
        /// Returns the vector with values set
        /// </summary>
        /// <param name="x">Sets the value of the X-axis</param>
        /// <param name="y">Sets the value of the Y-axis</param>
        public static Vector2 With(this Vector2 vector2, float? x = null, float? y = null){
            if (x.HasValue)
                vector2.x = x.Value;
            if (y.HasValue)
                vector2.y = y.Value;
            return vector2;
        }
        #endregion
    }
}