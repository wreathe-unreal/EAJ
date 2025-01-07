using UnityEditor;
using UnityEngine;

namespace AVM {
    public class DestroyMissileEditor : ISerializedPropertyDrawer {
        private SerializedProperty m_isActiveDestroyComp;
        private SerializedProperty m_destroyTimeMin;
        private SerializedProperty m_destroyTimeMax;
        private SerializedProperty m_canFallFlag;
        private SerializedProperty m_fallStartTime;
        private SerializedProperty m_drag;
        private bool fold = false;

        public DestroyMissileEditor(SerializedObject parent) {
            m_isActiveDestroyComp = parent.FindProperty("m_isActiveDestroyComp");
            m_destroyTimeMin = parent.FindProperty("m_destroyTimeMin");
            m_destroyTimeMax = parent.FindProperty("m_destroyTimeMax");
            m_canFallFlag = parent.FindProperty("m_canFallFlag");
            m_fallStartTime = parent.FindProperty("m_fallStartTime");
            m_drag = parent.FindProperty("m_drag");
        }

        public void DrawProperty() {
            EditorGUI.indentLevel = 0;
            bool activeRef = m_isActiveDestroyComp.boolValue;
            fold = AVMCustomFold.Foldout("Destroy(Fix)", fold, ref activeRef);
            m_isActiveDestroyComp.boolValue = true; //Fix Active
            if (fold) {
                EditorGUI.indentLevel = 1;
                EditorGUILayout.HelpBox("\"-1\" is infinity.", MessageType.None);
                EditorGUILayout.PropertyField(m_destroyTimeMin, new GUIContent("MinDestroyTime"));
                EditorGUILayout.PropertyField(m_destroyTimeMax, new GUIContent("MaxDestroyTime"));
                m_destroyTimeMin.floatValue = m_destroyTimeMin.floatValue < -1 ? -1 : m_destroyTimeMin.floatValue;
                m_destroyTimeMax.floatValue = m_destroyTimeMax.floatValue < m_destroyTimeMin.floatValue ? m_destroyTimeMin.floatValue : m_destroyTimeMax.floatValue;

                EditorGUILayout.PropertyField(m_canFallFlag, new GUIContent("LowPowerFall"));
                if (m_canFallFlag.boolValue) {
                    EditorGUI.indentLevel = 2;
                    EditorGUILayout.PropertyField(m_fallStartTime, new GUIContent("FallStartTime"));
                    m_fallStartTime.floatValue = m_fallStartTime.floatValue < 0.1f ? 0.1f : m_fallStartTime.floatValue;
                    EditorGUILayout.PropertyField(m_drag, new GUIContent("Drag"));
                }
            }
        }
    }
}