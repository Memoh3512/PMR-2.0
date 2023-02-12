using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace PMR.GraphEditor.Elements
{
    public class PMRNode : Node
    {
        public string NodeName { get; set; }
        public List<string> Inputs { get; set; }
        public List<string> Outputs { get; set; }

        public void Initialize()
        {
            NodeName = "NodeName";
        }

        public void Draw()
        {
            /* Title container */
            TextField nodeNameTextField = new TextField()
            {
                value = NodeName
            };
            
            titleContainer.Insert(0, nodeNameTextField);

            Port inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));

            inputPort.portName = "Input";
            
            inputContainer.Add(inputPort);
            
            RefreshExpandedState();
        }

    }
}
