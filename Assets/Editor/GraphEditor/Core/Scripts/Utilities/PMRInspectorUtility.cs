using System.Collections.Generic;
using UnityEditor;

namespace PMR.GraphEditor.Utilities
{
    public static class PMRInspectorUtility
    {
        public static void DrawHeader(string label)
        {
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
        }

        public static void DrawPropertyField(this SerializedProperty serializedProperty)
        {
            EditorGUILayout.PropertyField(serializedProperty);
        }

        public static int DrawPopup(string label, SerializedProperty selectedItemProperty, List<string> options)
        {
            if (string.IsNullOrEmpty(selectedItemProperty.stringValue) || !options.Contains(selectedItemProperty.stringValue))
            {
                selectedItemProperty.stringValue = options[0];
            }
            
            int index = EditorGUILayout.Popup(
                label,
                options.IndexOf(selectedItemProperty.stringValue),
                options.ToArray()
            );
            
            selectedItemProperty.stringValue = options[index];
            
            return index;
        }
        
        public static int DrawPopup(string label, int selectedIndex, string[] options)
        {
            return EditorGUILayout.Popup(label, selectedIndex, options);
        }

        public static void DrawSpace(int amount = 4)
        {
            EditorGUILayout.Space(amount);
        }

        public static void DrawHelpBox(string message, MessageType messageType = MessageType.Info, bool wide = true)
        {
            EditorGUILayout.HelpBox(message, messageType, wide);
        }
    }
}
