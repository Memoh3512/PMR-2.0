using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace PMR.GraphEditor
{
    public class PMRGraphEditorWindow : EditorWindow
    {
        [MenuItem("Window/PMR/Graph Editor")]
        public static void Open()
        {
            GetWindow<PMRGraphEditorWindow>("Graph Editor Window");
        }

        private void OnEnable()
        {
            AddGraphView();

            AddStyles();
        }

        private void AddGraphView()
        {
            PMRGraphView graphView = new PMRGraphView();
            
            graphView.StretchToParentSize();
            
            rootVisualElement.Add(graphView);
        }
        
        private void AddStyles()
        {
            StyleSheet stylesheet = (StyleSheet) EditorGUIUtility.Load("PMRGraphView/PMRVariables.uss");
            
            rootVisualElement.styleSheets.Add(stylesheet);
        }
    }   
}