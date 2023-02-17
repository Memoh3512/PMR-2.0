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

        private string defaultFilename;

        public PMRGraphEditorWindow()
        {
            defaultFilename = "GraphEditor";
        }

    protected static void Open<TGraphEditorWindow>(string title) where TGraphEditorWindow : PMRGraphEditorWindow
        {
            GetWindow<TGraphEditorWindow>(title);
        }

        protected virtual void OnEnable()
        {
            //AddGraphView<PMRGraphView>(); //Do this for every new graph editor
            AddToolbar();
            AddStyles();
        }

        protected void AddGraphView(PMRGraphView graphView)
        {
            graphView.StretchToParentSize();
            rootVisualElement.Add(graphView);
        }

        protected void AddToolbar()
        {
            Toolbar tb = new Toolbar();

            TextField fileNameTextField = PMRElementUtility.CreateTextField(defaultFilename, "File Name...");

            Button saveBtn = PMRElementUtility.CreateButton("Save");
            
            tb.Add(fileNameTextField);
            tb.Add(saveBtn);

            tb.AddStyleSheets("PMRGraphView/PMRToolbarStyles.uss");
            
            rootVisualElement.Add(tb);
        }
        
        private void AddStyles()
        {
            rootVisualElement.AddStyleSheets("PMRGraphView/PMRVariables.uss");
        }
    }   
}