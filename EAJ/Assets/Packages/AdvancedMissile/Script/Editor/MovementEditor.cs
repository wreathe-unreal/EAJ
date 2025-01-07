using UnityEngine;
using UnityEditor;

namespace AVM {
    public class MovementEditor : ISerializedPropertyDrawer {
        private SerializedProperty m_isActiveMovementComp;
        private SerializedProperty m_moveType;
        private SerializedProperty m_rotType;
        private SerializedProperty m_direction;
        private SerializedProperty m_forceMode;
        private SerializedProperty m_minPower;
        private SerializedProperty m_maxPower;
        private SerializedProperty m_minSpeed;
        private SerializedProperty m_maxSpeed;
        private SerializedProperty m_anglePerSecond;
        private SerializedProperty m_torquePower;
        private bool fold = false;

        public MovementEditor(SerializedObject parent) {
            m_isActiveMovementComp = parent.FindProperty("m_isActiveMovementComp");
            m_moveType = parent.FindProperty("m_moveType");
            m_rotType = parent.FindProperty("m_rotType");
            m_direction = parent.FindProperty("m_direction");
            m_forceMode = parent.FindProperty("m_forceMode");
            m_minPower = parent.FindProperty("m_minPower");
            m_maxPower = parent.FindProperty("m_maxPower");
            m_minSpeed = parent.FindProperty("m_minSpeed");
            m_maxSpeed = parent.FindProperty("m_maxSpeed");
            m_anglePerSecond = parent.FindProperty("m_anglePerSecond");
            m_torquePower = parent.FindProperty("m_torquePower");
        }

        public void DrawProperty() {
            EditorGUI.indentLevel = 0;
            bool activeRef = m_isActiveMovementComp.boolValue;
            fold = AVMCustomFold.Foldout("Movement", fold, ref activeRef);
            m_isActiveMovementComp.boolValue = activeRef;
            if (fold) {
                EditorGUI.indentLevel = 1;
                EditorGUILayout.PropertyField(m_direction, new GUIContent("Direction"));
                EditorGUILayout.PropertyField(m_moveType, new GUIContent("MoveType"));
                EditorGUI.indentLevel = 2;
                switch (m_moveType.enumValueIndex) {
                    case (int)AdvancedMissile.MoveTypes.Translate:
                        EditorGUILayout.PropertyField(m_minSpeed, new GUIContent("MinSpeed"));
                        EditorGUILayout.PropertyField(m_maxSpeed, new GUIContent("MaxSpeed"));
                        m_minSpeed.floatValue = m_minSpeed.floatValue < 0 ? 0 : m_minSpeed.floatValue;
                        m_maxSpeed.floatValue = m_maxSpeed.floatValue < m_minSpeed.floatValue ? m_minSpeed.floatValue : m_maxSpeed.floatValue;
                        break;
                    case (int)AdvancedMissile.MoveTypes.AddForce:
                        EditorGUILayout.PropertyField(m_forceMode, new GUIContent("ForceMode"));
                        EditorGUILayout.PropertyField(m_minPower, new GUIContent("MinPower"));
                        EditorGUILayout.PropertyField(m_maxPower, new GUIContent("MaxPower"));
                        m_minPower.floatValue = m_minPower.floatValue < 0 ? 0 : m_minPower.floatValue;
                        m_maxPower.floatValue = m_maxPower.floatValue < m_minPower.floatValue ? m_minPower.floatValue : m_maxPower.floatValue;
                        break;
                }
                EditorGUI.indentLevel = 1;
                EditorGUILayout.PropertyField(m_rotType, new GUIContent("RotateType"));
                EditorGUI.indentLevel = 2;
                switch (m_rotType.enumValueIndex) {
                    case (int)AdvancedMissile.RotateTypes.PerSecond:
                        EditorGUILayout.PropertyField(m_anglePerSecond, new GUIContent("Angle"));
                        break;
                    case (int)AdvancedMissile.RotateTypes.Torque:
                        EditorGUILayout.PropertyField(m_torquePower, new GUIContent("Torque"));
                        break;
                }
            }
        }
    }
}