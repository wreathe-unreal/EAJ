using UnityEngine;
using UnityEditor;

namespace AVM {
    public class CreateAvoidanceNodeEditor : ISerializedPropertyDrawer {
        private SerializedProperty m_isActiveAvoidComp;
        private SerializedProperty m_enableAvoid;
        private SerializedProperty m_distanceBetweenObstacle;
        private SerializedProperty m_createInterval;
        private SerializedProperty m_canCreateUpRoute;
        private SerializedProperty m_canCreateDownRoute;
        private SerializedProperty m_drawRoute;
        private bool fold = false;

        public CreateAvoidanceNodeEditor(SerializedObject parent) {
            m_isActiveAvoidComp = parent.FindProperty("m_isActiveAvoidComp");
            m_enableAvoid = parent.FindProperty("m_enableAvoid");
            m_distanceBetweenObstacle = parent.FindProperty("m_distanceBetweenObstacle");
            m_createInterval = parent.FindProperty("m_createInterval");
            m_canCreateUpRoute = parent.FindProperty("m_canCreateUpRoute");
            m_canCreateDownRoute = parent.FindProperty("m_canCreateDownRoute");
            m_drawRoute = parent.FindProperty("m_drawRoute");
        }

        public void DrawProperty() {
            EditorGUI.indentLevel = 0;
            bool activeRef = m_isActiveAvoidComp.boolValue;
            fold = AVMCustomFold.Foldout("Avoid", fold, ref activeRef);
            m_isActiveAvoidComp.boolValue = activeRef;
            if (fold) {
                EditorGUI.indentLevel = 1;
                EditorGUILayout.PropertyField(m_enableAvoid, new GUIContent("EnableAvoidance"));
                if (m_enableAvoid.boolValue) {
                    EditorGUI.indentLevel = 2;
                    EditorGUILayout.PropertyField(m_distanceBetweenObstacle, new GUIContent("DistanceBetweenObstacle"));
                    EditorGUILayout.PropertyField(m_createInterval, new GUIContent("ReCreateInterval"));
                    EditorGUILayout.PropertyField(m_canCreateUpRoute, new GUIContent("CreateUpRoute"));
                    EditorGUILayout.PropertyField(m_canCreateDownRoute, new GUIContent("CreateDownRoute"));
                    EditorGUILayout.PropertyField(m_drawRoute, new GUIContent("DrawRoute"));
                }
            }
        }
    }
}