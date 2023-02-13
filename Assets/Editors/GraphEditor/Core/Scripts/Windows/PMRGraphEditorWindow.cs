using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace PMR.GraphEditor
{
    using Utilities;
    public class PMRGraphEditorWindow : EditorWindow
    {
        protected static void Open<TGraphEditorWindow>(string title) where TGraphEditorWindow : PMRGraphEditorWindow
        {
            GetWindow<TGraphEditorWindow>(title);
        }

        protected virtual void OnEnable()
        {
            //AddGraphView<PMRGraphView>(); //Do this for every new graph editor
            AddStyles();
        }

        protected void AddGraphView(PMRGraphView graphView)
        {
            graphView.StretchToParentSize();
            rootVisualElement.Add(graphView);
        }
        
        private void AddStyles()
        {
            rootVisualElement.AddStyleSheets("PMRGraphView/PMRVariables.uss");
        }
    }   
}