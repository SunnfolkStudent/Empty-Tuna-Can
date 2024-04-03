using System;
using UnityEditor;
using UnityEngine;

namespace Utils.CustomAttributes {
    [AttributeUsage(AttributeTargets.Field)]
    public class AssetPreviewIconAttribute : PropertyAttribute {}
    
    #if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(AssetPreviewIconAttribute))]
    public class AssetPreviewIconDrawer : PropertyDrawer {
        private static float _iconSize = 128;
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.PropertyField(position, property, label);
            
            var icon = AssetPreview.GetAssetPreview(property.objectReferenceValue);
            if (icon == null) {
                Debug.LogWarning("AssetPreview icon was not found.");
                return;
            }
            
            var sliderRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight, position.width, EditorGUIUtility.singleLineHeight);
            _iconSize = EditorGUI.Slider(sliderRect, "Icon Size", _iconSize, 64, 128);
            
            var iconRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 2, _iconSize, _iconSize);
            
            GUI.DrawTexture(iconRect, icon);
            GUILayout.Space(GetPropertyHeight(property, label));
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            var baseHeight = base.GetPropertyHeight(property, label);
            return baseHeight + _iconSize;
        }
    }
    #endif
}