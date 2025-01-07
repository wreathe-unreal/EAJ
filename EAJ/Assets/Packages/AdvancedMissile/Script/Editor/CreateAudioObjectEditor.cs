using UnityEngine;
using UnityEditor;

namespace AVM {
    public class CreateAudioObjectEditor : ISerializedPropertyDrawer {
        private SerializedProperty m_isActiveAudioComp;
        private SerializedProperty m_shotSE;
        private SerializedProperty m_shotSEVolume;
        private SerializedProperty m_moveSE;
        private SerializedProperty m_moveSEVolume;
        private SerializedProperty m_explosionSE;
        private SerializedProperty m_explosionSEVolume;
        private bool fold = false;

        public CreateAudioObjectEditor(SerializedObject parent) {
            m_isActiveAudioComp = parent.FindProperty("m_isActiveAudioComp");
            m_shotSE = parent.FindProperty("m_shotSE");
            m_shotSEVolume = parent.FindProperty("m_shotSEVolume");
            m_moveSE = parent.FindProperty("m_moveSE");
            m_moveSEVolume = parent.FindProperty("m_moveSEVolume");
            m_explosionSE = parent.FindProperty("m_explosionSE");
            m_explosionSEVolume = parent.FindProperty("m_explosionSEVolume");
        }

        public void DrawProperty() {
            EditorGUI.indentLevel = 0;
            bool activeRef = m_isActiveAudioComp.boolValue;
            fold = AVMCustomFold.Foldout("Audio", fold, ref activeRef);
            m_isActiveAudioComp.boolValue = activeRef;
            if (fold) {
                EditorGUI.indentLevel = 1;
                EditorGUILayout.PropertyField(m_shotSE, new GUIContent("ShotSE"));
                EditorGUI.indentLevel = 2;
                EditorGUILayout.PropertyField(m_shotSEVolume, new GUIContent("Volume"));
                EditorGUI.indentLevel = 1;
                EditorGUILayout.PropertyField(m_moveSE, new GUIContent("MoveSE"));
                EditorGUI.indentLevel = 2;
                EditorGUILayout.PropertyField(m_moveSEVolume, new GUIContent("Volume"));
                EditorGUI.indentLevel = 1;
                EditorGUILayout.PropertyField(m_explosionSE, new GUIContent("ExplosionSE"));
                EditorGUI.indentLevel = 2;
                EditorGUILayout.PropertyField(m_explosionSEVolume, new GUIContent("Volume"));
            }
        }
    }
}