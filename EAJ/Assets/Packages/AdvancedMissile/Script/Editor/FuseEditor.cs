using UnityEngine;
using UnityEditor;

namespace AVM {
    public class FuseEditor : ISerializedPropertyDrawer {
        private SerializedProperty m_isActiveFuseComp;
        private SerializedProperty m_fuseType;
        private SerializedProperty m_time;
        private SerializedProperty m_distance;
        private SerializedProperty m_height;
        private SerializedProperty m_heightType;
        private SerializedProperty m_layerMask; 
        private bool fold = false;

        public FuseEditor(SerializedObject parent) {
            m_isActiveFuseComp = parent.FindProperty("m_isActiveFuseComp");
            m_fuseType = parent.FindProperty("m_fuseType");
            m_time = parent.FindProperty("m_time");
            m_distance = parent.FindProperty("m_targetDistance");
            m_height = parent.FindProperty("m_height");
            m_heightType = parent.FindProperty("m_heightType");
            m_layerMask = parent.FindProperty("m_layerMask");
        }

        public void DrawProperty() {
            
            EditorGUI.indentLevel = 0;
            bool activeRef = m_isActiveFuseComp.boolValue;
            fold = AVMCustomFold.Foldout("Fuse(Fix)", fold, ref activeRef);
            m_isActiveFuseComp.boolValue = true;
            if (fold) {
                EditorGUI.indentLevel = 1;
                EditorGUILayout.PropertyField(m_fuseType, new GUIContent("FuseType"));
                EditorGUI.indentLevel = 2;
                switch (m_fuseType.enumValueIndex) {
                    case (int)AdvancedMissile.FuseTypes.Time:
                        EditorGUILayout.PropertyField(m_time, new GUIContent("Time"));
                        break;
                    case (int)AdvancedMissile.FuseTypes.Proximity:
                        EditorGUILayout.PropertyField(m_distance, new GUIContent("Distance"));
                        break;
                    case (int)AdvancedMissile.FuseTypes.Height:
                        EditorGUILayout.PropertyField(m_heightType, new GUIContent("HeightType"));
                        if (m_heightType.enumValueIndex == 1) {
                            EditorGUILayout.PropertyField(m_layerMask, new GUIContent("LayerMask"));
                        }
                        EditorGUILayout.PropertyField(m_height, new GUIContent("Height"));
                        break;
                }
            }
        }
    }
}