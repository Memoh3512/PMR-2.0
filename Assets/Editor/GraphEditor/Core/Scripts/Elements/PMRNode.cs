using System;
using System.Collections.Generic;
using PMR.GraphEditor.Save;
using PMR.ScriptableObjects;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace PMR.GraphEditor.Elements
{
    using Utilities;
    
    public class PMRNode : Node
    {

        [ReadOnly] public string ID;
        public string NodeName { get; set; }
        public PMRGroup Group { get; set; }

        protected PMRGraphView graphView;

        public virtual void Initialize(string nodeName, PMRGraphView pmrGraphView, Vector2 position)
        {
            ID = Guid.NewGuid().ToString();
            NodeName = nodeName;

            graphView = pmrGraphView;
                
            SetPosition(new Rect(position, Vector2.zero));
            
            mainContainer.AddToClassList("ds-node__main-container");
            extensionContainer.AddToClassList("ds-node__extension-container");
        }

        public virtual void Draw()
        {
            /* Title container */
            TextField nodeNameTextField = PMRElementUtility.CreateTextField(NodeName, null, callback =>
            {
                NodeName = callback.newValue;
            });
            nodeNameTextField.AddClasses(
                "ds-node__text-field",
                "ds-node__filename-text-field",
                "ds-node__text-field__hidden");

            
            titleContainer.Insert(0, nodeNameTextField);
            
        }

        public virtual PMRNodeSaveData CreateEditorSaveData()
        {
            throw new Exception("CreateEditorSaveData called in PMRNode! This should never happen and should always be overridden with \"override\" keyword!");
        }
        public virtual PMRGraphSO CreateRuntimeSaveData(string path, string fileName)
        {
            throw new Exception("CreateRuntimeSaveData called in PMRNode! This should never happen and should always be overridden with \"override\" keyword!");
        }
        public virtual void SaveConnections(PMRGraphSO nodeSo, Dictionary<string, PMRGraphSO> createdNodes)
        {
            throw new Exception("SaveConnections called in PMRNode! This should never happen and should always be overridden with \"override\" keyword!");
        }
        public virtual void LoadConnections(Dictionary<string, PMRNode> loadedNodes)
        {
            throw new Exception("LoadConnections called in PMRNode! This should never happen and should always be overridden with \"override\" keyword!");
        }
    }
}
