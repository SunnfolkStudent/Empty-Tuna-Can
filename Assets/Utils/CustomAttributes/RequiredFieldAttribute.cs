using System;
using UnityEditor;
using UnityEngine;

namespace Utils.CustomAttributes {
    [AttributeUsage(AttributeTargets.Field)]
    public class RequiredFieldAttribute : PropertyAttribute {}
    
    #if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(RequiredFieldAttribute))]
    public class RequiredFieldAttributeDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.PropertyField(position, property, label);
            
            if (!IsValueNullOrEmpty(property)) return;
            var width = position.width / 2.5f;
            var rect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight, width, EditorGUIUtility.singleLineHeight);
            
            GUILayout.Space(EditorGUIUtility.singleLineHeight);
            EditorGUI.HelpBox(rect, "Field is required.", MessageType.Warning);
            Debug.LogWarning($"Required field '{ObjectNames.NicifyVariableName(property.name)}' has not been set.");
        }

        private static bool IsValueNullOrEmpty(SerializedProperty property) {
            return property.propertyType switch {
                SerializedPropertyType.ObjectReference => property.objectReferenceValue == null,
                SerializedPropertyType.String => string.IsNullOrEmpty(property.stringValue),
                SerializedPropertyType.ArraySize => property.arraySize == 0,
                SerializedPropertyType.Float => property.floatValue == 0,
                _ => false
            };
        }
    }
    #endif
}