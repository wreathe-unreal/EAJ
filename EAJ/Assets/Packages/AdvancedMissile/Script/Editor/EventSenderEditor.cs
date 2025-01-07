using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace AVM {
    public class EventSenderEditor : ISerializedPropertyDrawer {
        private SerializedObject m_parent;
        private List<SerializedProperty> m_eventSendValueList;
        private SerializedProperty m_isActiveEventComp;
        private SerializedProperty m_callMethodName;
        private SerializedProperty m_eventValues;
        private SerializedProperty m_intValue;
        private SerializedProperty m_floatValue;
        private SerializedProperty m_stringValue;
        private SerializedProperty m_boolValue;
        private SerializedProperty m_vector2Value;
        private SerializedProperty m_vector3Value;
        private SerializedProperty m_gameObjectValue;
        private SerializedProperty m_audioClipValue;
        private bool fold = false;

        public EventSenderEditor(SerializedObject parent) {
            m_isActiveEventComp = parent.FindProperty("m_isActiveEventComp");
            m_callMethodName = parent.FindProperty("m_callMethodName");
            m_eventValues = parent.FindProperty("m_eventValues");
            m_intValue = parent.FindProperty("m_intValue");
            m_floatValue = parent.FindProperty("m_floatValue");
            m_stringValue = parent.FindProperty("m_stringValue");
            m_boolValue = parent.FindProperty("m_boolValue");
            m_vector2Value = parent.FindProperty("m_vector2Value");
            m_vector3Value = parent.FindProperty("m_vector3Value");
            m_gameObjectValue = parent.FindProperty("m_gameObjectValue");
            m_audioClipValue = parent.FindProperty("m_audioClipValue");
        }

        public void DrawProperty() {
            EditorGUI.indentLevel = 0;
            bool activeRef = m_isActiveEventComp.boolValue;
            fold = AVMCustomFold.Foldout("Event", fold, ref activeRef);
            m_isActiveEventComp.boolValue = activeRef;
            if (fold) {
                EditorGUI.indentLevel = 1;
                EditorGUILayout.PropertyField(m_callMethodName, new GUIContent("CallMethod"));
                EditorGUILayout.PropertyField(m_eventValues, new GUIContent("SendValueType"));

                if (m_eventSendValueList == null || m_eventSendValueList.Count == 0) {
                    m_eventSendValueList = new List<SerializedProperty>();
                    m_eventSendValueList.Add(m_intValue);
                    m_eventSendValueList.Add(m_floatValue);
                    m_eventSendValueList.Add(m_stringValue);
                    m_eventSendValueList.Add(m_boolValue);
                    m_eventSendValueList.Add(m_vector2Value);
                    m_eventSendValueList.Add(m_vector3Value);
                    m_eventSendValueList.Add(m_gameObjectValue);
                    m_eventSendValueList.Add(m_audioClipValue);
                }
                if (m_eventValues.enumValueIndex != 0) {
                    EditorGUI.indentLevel = 2;
                    EditorGUILayout.PropertyField(m_eventSendValueList[m_eventValues.enumValueIndex - 1], new GUIContent("Value"));
                }
            }
        }
    }
}