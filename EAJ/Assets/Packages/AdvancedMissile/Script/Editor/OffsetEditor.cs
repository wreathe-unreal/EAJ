using UnityEngine;
using UnityEditor;

namespace AVM {
    public class OffsetEditor : ISerializedPropertyDrawer {
        private SerializedProperty m_isActiveOffsetComp;
        private SerializedProperty m_offset;
        private SerializedProperty m_activeRandomOffset;
        private SerializedProperty m_randomAmplitude;
        private SerializedProperty m_randomOffsetX;
        private SerializedProperty m_randomOffsetY;
        private SerializedProperty m_randomOffsetZ;
        private SerializedProperty m_randomOffsetIntervalMin;
        private SerializedProperty m_randomOffsetIntervalMax;
        private bool fold = false;

        public OffsetEditor(SerializedObject parent) {
            m_isActiveOffsetComp = parent.FindProperty("m_isActiveOffsetComp");
            m_offset = parent.FindProperty("m_offset");
            m_activeRandomOffset = parent.FindProperty("m_activeRandomOffset");
            m_randomAmplitude = parent.FindProperty("m_randomAmplitude");
            m_randomOffsetX = parent.FindProperty("m_randomOffsetX");
            m_randomOffsetY = parent.FindProperty("m_randomOffsetY");
            m_randomOffsetZ = parent.FindProperty("m_randomOffsetZ");
            m_randomOffsetIntervalMin = parent.FindProperty("m_randomOffsetIntervalMin");
            m_randomOffsetIntervalMax = parent.FindProperty("m_randomOffsetIntervalMax");
        }

        public void DrawProperty() {
            EditorGUI.indentLevel = 0;
            bool activeRef = m_isActiveOffsetComp.boolValue;
            fold = AVMCustomFold.Foldout("Offset", fold, ref activeRef);
            m_isActiveOffsetComp.boolValue = activeRef;
            if (fold) {
                EditorGUI.indentLevel = 1;
                EditorGUILayout.PropertyField(m_offset, new GUIContent("Offset"));
                EditorGUILayout.PropertyField(m_activeRandomOffset, new GUIContent("ActiveRandomOffset"));

                if (m_activeRandomOffset.boolValue) {
                    EditorGUI.indentLevel = 2;
                    EditorGUILayout.PropertyField(m_randomAmplitude, new GUIContent("Amplitude"));
                    EditorGUILayout.PropertyField(m_randomOffsetX, new GUIContent("RandomOffsetX"));
                    EditorGUILayout.PropertyField(m_randomOffsetY, new GUIContent("RandomOffsetY"));
                    EditorGUILayout.PropertyField(m_randomOffsetZ, new GUIContent("RandomOffsetZ"));
                    EditorGUILayout.PropertyField(m_randomOffsetIntervalMin, new GUIContent("MinInterval"));
                    EditorGUILayout.PropertyField(m_randomOffsetIntervalMax, new GUIContent("MaxInterval"));
                    m_randomOffsetIntervalMin.floatValue = m_randomOffsetIntervalMin.floatValue < 0 ? 0 : m_randomOffsetIntervalMin.floatValue;
                    m_randomOffsetIntervalMax.floatValue = m_randomOffsetIntervalMax.floatValue < 0 ? 0 : m_randomOffsetIntervalMax.floatValue;
                    if (m_randomOffsetIntervalMax.floatValue < m_randomOffsetIntervalMin.floatValue) {
                        m_randomOffsetIntervalMax.floatValue = m_randomOffsetIntervalMin.floatValue;
                    }
                }
            }
        }
    }
}