using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace PMR.GraphEditor
{
    using Utilities;
    public class PMRGraphEditorWindow : EditorWindow
    {

        private PMRGraphView graphView;
        protected string defaultFilename;
        protected string folderName;

        private Button minimapBtn;

        private static TextField fileNameTextField;

        public PMRGraphEditorWindow()
        {
            folderName = "BaseGraphEditor";
            defaultFilename = "MyGraphEditor";
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
            this.graphView = graphView;
            graphView.StretchToParentSize();
            rootVisualElement.Add(graphView);
        }

        protected void AddToolbar()
        {
            Toolbar tb = new Toolbar();

            fileNameTextField = PMRElementUtility.CreateTextField(defaultFilename, "File Name...");

            Button saveBtn = PMRElementUtility.CreateButton("Save", Save);

            Button loadBtn = PMRElementUtility.CreateButton("Load...", Load);
            Button clearBtn = PMRElementUtility.CreateButton("Clear", Clear);
            Button newBtn = PMRElementUtility.CreateButton("New", ResetGraph);
            minimapBtn = PMRElementUtility.CreateButton("Minimap", ToggleMinimap);
            
            tb.Add(fileNameTextField);
            tb.Add(saveBtn);
            tb.Add(loadBtn);
            tb.Add(clearBtn);
            tb.Add(newBtn);
            tb.Add(minimapBtn);

            tb.AddStyleSheets("PMRGraphView/PMRToolbarStyles.uss");
            
            rootVisualElement.Add(tb);
        }

        private void Clear()
        {
            graphView.ClearGraph();
        }
        
        private void ResetGraph()
        {
            Clear();
            UpdateFileName(defaultFilename);
        }

        private void Save()
        {

            if (string.IsNullOrEmpty(fileNameTextField.value))
            {
                EditorUtility.DisplayDialog(
                    "Invalid File Name",
                    "Please ensure that the file name entered is valid.",
                    "Ok");
                return;
            }
            
            PMRIOUtility.Initialize(graphView, folderName, fileNameTextField.value);
            PMRIOUtility.Save();
        }

        private void Load()
        {
            string filePath = EditorUtility.OpenFilePanel("Choose Graph...", $"Assets/Editor/Graphs/{folderName}", "asset");

            if (!string.IsNullOrEmpty(filePath))
            {
                Clear();
                PMRIOUtility.Initialize(graphView, folderName, Path.GetFileNameWithoutExtension(filePath));
                PMRIOUtility.Load();
            }
        }

        private void ToggleMinimap()
        {
            graphView.ToggleMinimap();
            minimapBtn.ToggleInClassList("ds-toolbar__button__selected");
        }

        public static void UpdateFileName(string newFileName)
        {
            fileNameTextField.value = newFileName;
        }
        
        private void AddStyles()
        {
            rootVisualElement.AddStyleSheets("PMRGraphView/PMRVariables.uss");
        }
    }   
}