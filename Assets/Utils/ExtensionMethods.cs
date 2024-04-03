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
        /// <param name="timeInSeconds">The time in seconds to format.</param>
        /// <param name="format">The string format to use for the time representation.</param>
        /// <returns>A string representing the time in the specified format.</returns>
        public static string ToStringTimeFormat(this float timeInSeconds, string format) {
            return TimeSpan.FromSeconds(timeInSeconds).ToString(format);
        } 
        
        /// <summary>
        /// Selects a random element from a list or array.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The read-only list from which to select a random element.</param>
        /// <returns>A randomly selected element from the list.</returns>
        public static T GetRandom<T>(this IReadOnlyList<T> list) {
            var random = new System.Random();
            return list[random.Next(0, list.Count)];
        }
    }
}