using UnityEngine;
using UnityEditor;

namespace AVM {
    public class MissileColliderEditor : ISerializedPropertyDrawer {
        private SerializedProperty m_isActiveCollisionComp;
        private SerializedProperty m_enableCollision;
        private SerializedProperty m_collideEachOther;
        private SerializedProperty m_enableCollisionInterval;
        private SerializedProperty m_rayCollider;
        private SerializedProperty m_capsuleLinerRadius;
        private bool fold = false;

        public MissileColliderEditor(SerializedObject parent) {
            m_isActiveCollisionComp = parent.FindProperty("m_isActiveCollisionComp");
            m_enableCollision = parent.FindProperty("m_enableCollision");
            m_collideEachOther = parent.FindProperty("m_collideEachOther");
            m_enableCollisionInterval = parent.FindProperty("m_enableCollisionInterval");
            m_rayCollider = parent.FindProperty("m_rayCollider");
            m_capsuleLinerRadius = parent.FindProperty("m_capsuleLinerRadius");
        }

        public void DrawProperty() {
            EditorGUI.indentLevel = 0;
            bool activeRef = m_isActiveCollisionComp.boolValue;
            fold = AVMCustomFold.Foldout("Collision", fold, ref activeRef);
            m_isActiveCollisionComp.boolValue = activeRef;
            if (fold) {
                EditorGUI.indentLevel = 1;
                EditorGUILayout.PropertyField(m_enableCollision, new GUIContent("EnableCollision"));
                if (m_enableCollision.boolValue) {
                    EditorGUI.indentLevel = 2;
                    EditorGUILayout.PropertyField(m_enableCollisionInterval, new GUIContent("EnableInterval"));
                    EditorGUILayout.PropertyField(m_collideEachOther, new GUIContent("CollideEachOther"));
                    EditorGUILayout.PropertyField(m_rayCollider, new GUIContent("ColliderType"));
                    if (m_rayCollider.enumValueIndex == 2) {
                        EditorGUI.indentLevel = 3;
                        EditorGUILayout.PropertyField(m_capsuleLinerRadius, new GUIContent("Radius"));
                    }
                }
            }
        }
    }
}