using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AVM {
    public static class AVMCustomFold {

        public static bool Foldout(string title, bool display, ref bool out_activeToggle) {
            var style = new GUIStyle("ShurikenModuleTitle");
            style.font = new GUIStyle(EditorStyles.label).font;
            style.border = new RectOffset(15, 7, 4, 4);
            style.fixedHeight = 22;
            style.contentOffset = new Vector2(20f, -2f);

            var rect = GUILayoutUtility.GetRect(16f, 22f, style);
            GUI.Box(rect, title, style);

            var e = UnityEngine.Event.current;

            var checkBoxToggleRect = new Rect(rect.x + 4f, rect.y + 2f, 13f, 13f);

            if (e.type == EventType.MouseDown) {
                if (checkBoxToggleRect.Contains(e.mousePosition)) {
                    out_activeToggle = !out_activeToggle;
                    e.Use();
                } else if (rect.Contains(e.mousePosition)) {
                    display = !display;
                    e.Use();
                }
            }

            if (e.type == EventType.Repaint) {
                EditorStyles.toggle.Draw(checkBoxToggleRect, false, true, out_activeToggle, false);
            }

            return display;
        }
    }
}