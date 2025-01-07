using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace AVM {
    public class SearchTargetEditor : ISerializedPropertyDrawer {
        private ReorderableList m_reorderTagList;
        private ReorderableList m_reorderNameList;
        private SerializedProperty m_searchType;
        private SerializedProperty m_isActiveSearchComp;
        private SerializedProperty m_canResearch;
        private SerializedProperty m_searchInterval;
        private SerializedProperty m_sight;
        private SerializedProperty m_activeFollowInterval;
        private bool fold = false;

        public SearchTargetEditor(SerializedObject parent) {
            m_isActiveSearchComp = parent.FindProperty("m_isActiveSearchComp");
            m_searchType = parent.FindProperty("m_searchType");
            m_canResearch = parent.FindProperty("m_canResearch");
            m_searchInterval = parent.FindProperty("m_searchInterval");
            m_sight = parent.FindProperty("m_sight");
            m_activeFollowInterval = parent.FindProperty("m_activeFollowInterval");
            var tagProp = parent.FindProperty("m_targetTags");
            m_reorderTagList = new ReorderableList(parent, tagProp);
            m_reorderTagList.drawElementCallback = (rect, index, isActive, isFocused) => {
                var element = tagProp.GetArrayElementAtIndex(index);
                rect.height -= 4;
                rect.y += 2;
                EditorGUI.PropertyField(rect, element);
            };
            var nameProp = parent.FindProperty("m_targetNames");
            m_reorderNameList = new ReorderableList(parent, nameProp);
            m_reorderNameList.drawElementCallback = (rect, index, isActive, isFocused) => {
                var element = nameProp.GetArrayElementAtIndex(index);
                rect.height -= 4;
                rect.y += 2;
                EditorGUI.PropertyField(rect, element);
            };
        }

        public void DrawProperty() {
            EditorGUI.indentLevel = 0;
            bool activeRef = m_isActiveSearchComp.boolValue;
            fold = AVMCustomFold.Foldout("Search", fold, ref activeRef);
            m_isActiveSearchComp.boolValue = activeRef;
            if (fold) {
                EditorGUI.indentLevel = 1;
                EditorGUILayout.PropertyField(m_activeFollowInterval, new GUIContent("ActiveFollowInterval"));
                EditorGUILayout.PropertyField(m_searchType, new GUIContent("SearchType"));
                switch (m_searchType.enumValueIndex) {
                    case (int)AdvancedMissile.SearchTypes.Tag:
                        m_reorderTagList.DoLayoutList();
                        break;
                    case (int)AdvancedMissile.SearchTypes.Name:
                        m_reorderNameList.DoLayoutList();
                        break;
                }
                EditorGUILayout.PropertyField(m_canResearch, new GUIContent("CanResearch"));
                if (m_canResearch.boolValue) {
                    EditorGUI.indentLevel = 2;
                    EditorGUILayout.PropertyField(m_searchInterval, new GUIContent("Interval"));
                }
                EditorGUILayout.PropertyField(m_sight, new GUIContent("SightAngle"));
            }
        }
    }
}