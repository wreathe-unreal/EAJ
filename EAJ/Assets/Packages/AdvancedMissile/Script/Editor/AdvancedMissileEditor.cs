using System.Collections.Generic;
using UnityEditor;

namespace AVM {
    [CustomEditor(typeof(AdvancedMissile))]
    public class AdvancedMissileEditor : Editor {
        private List<ISerializedPropertyDrawer> m_avmEditorList;

        void OnEnable() {
            m_avmEditorList = new List<ISerializedPropertyDrawer>();
            m_avmEditorList.Add(new DestroyMissileEditor(serializedObject));
            m_avmEditorList.Add(new FuseEditor(serializedObject));
            m_avmEditorList.Add(new MovementEditor(serializedObject));
            m_avmEditorList.Add(new SearchTargetEditor(serializedObject));
            m_avmEditorList.Add(new MissileColliderEditor(serializedObject));
            m_avmEditorList.Add(new OffsetEditor(serializedObject));
            m_avmEditorList.Add(new CreateAvoidanceNodeEditor(serializedObject));
            m_avmEditorList.Add(new CreateAudioObjectEditor(serializedObject));
            m_avmEditorList.Add(new CreateEffectEditor(serializedObject));
            m_avmEditorList.Add(new EventSenderEditor(serializedObject));
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();
            {
                EditorGUILayout.Space();
                foreach (ISerializedPropertyDrawer editor in m_avmEditorList) {
                    editor.DrawProperty();
                }
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}