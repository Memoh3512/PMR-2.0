using System;
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

        protected PMRGraphView graphView;

        public virtual void Initialize(PMRGraphView pmrGraphView, Vector2 position)
        {
            ID = Guid.NewGuid().ToString();
            NodeName = "NodeName";

            graphView = pmrGraphView;
                
            SetPosition(new Rect(position, Vector2.zero));
            
            mainContainer.AddToClassList("ds-node__main-container");
            extensionContainer.AddToClassList("ds-node__extension-container");
        }

        public virtual void Draw()
        {
            /* Title container */
            TextField nodeNameTextField = PMRElementUtility.CreateTextField(NodeName);
            nodeNameTextField.AddClasses(
                "ds-node__text-field",
                "ds-node__filename-text-field",
                "ds-node__text-field__hidden");

            
            titleContainer.Insert(0, nodeNameTextField);
            
        }

    }
}
