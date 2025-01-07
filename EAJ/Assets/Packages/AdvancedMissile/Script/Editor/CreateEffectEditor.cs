using UnityEngine;
using UnityEditor;

namespace AVM {
    public class CreateEffectEditor : ISerializedPropertyDrawer {
        private SerializedProperty m_isActiveEffectComp;
        private SerializedProperty m_smokeOffset;
        private SerializedProperty m_keepChild;
        private SerializedProperty m_smokeDestroyTime;
        private SerializedProperty m_smokeEffect;
        private SerializedProperty m_explosionEffect;
        private SerializedProperty m_explosionScale;
        private bool fold = false;

        public CreateEffectEditor(SerializedObject parent) {
            m_isActiveEffectComp = parent.FindProperty("m_isActiveEffectComp");
            m_smokeOffset = parent.FindProperty("m_smokeOffset");
            m_keepChild = parent.FindProperty("m_keepChild");
            m_smokeDestroyTime = parent.FindProperty("m_smokeDestroyTime");
            m_smokeEffect = parent.FindProperty("m_smokeEffect");
            m_explosionEffect = parent.FindProperty("m_explosionEffect");
            m_explosionScale = parent.FindProperty("m_explosionScale");
        }

        public void DrawProperty() {
            EditorGUI.indentLevel = 0;
            bool activeRef = m_isActiveEffectComp.boolValue;
            fold = AVMCustomFold.Foldout("Effect", fold, ref activeRef);
            m_isActiveEffectComp.boolValue = activeRef;
            if (fold) {
                EditorGUI.indentLevel = 1;
                EditorGUILayout.PropertyField(m_smokeEffect, new GUIContent("SmokeEffect"));
                if (m_smokeEffect.objectReferenceValue != null) {
                    EditorGUI.indentLevel = 2;
                    EditorGUILayout.PropertyField(m_smokeOffset, new GUIContent("Offset"));
                    EditorGUILayout.PropertyField(m_keepChild, new GUIContent("KeepChild"));
                    if (!m_keepChild.boolValue) {
                        EditorGUI.indentLevel = 3;
                        EditorGUILayout.PropertyField(m_smokeDestroyTime, new GUIContent("DestroyTime"));
                    }
                }
                EditorGUI.indentLevel = 1;
                EditorGUILayout.PropertyField(m_explosionEffect, new GUIContent("ExplosionEffect"));
                EditorGUILayout.PropertyField(m_explosionScale, new GUIContent("ExplosionScale"));
            }
        }
    }
}