using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace PMR.GraphEditor.Elements
{
    using Utilities;
    
    public class PMRNode : Node
    {
        public string NodeName { get; set; }

        public virtual void Initialize(Vector2 position)
        {
            NodeName = "NodeName";
            
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
