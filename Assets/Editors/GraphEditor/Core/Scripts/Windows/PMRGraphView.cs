using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace PMR.GraphEditor
{
    using Elements;
    using Utilities;
    public class PMRGraphView : GraphView
    {
        public PMRGraphView()
        {
            AddManipulators();
            AddGridBackground();
            AddStyles();
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatiblePorts = new List<Port>();

            ports.ForEach(port =>
            {
                if (startPort == port) return;
                if (startPort.node == port.node) return;
                if (startPort.direction == port.direction) return;
                compatiblePorts.Add(port);
            });
            
            return compatiblePorts;
        }

        private void AddManipulators()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            
            this.AddManipulator(CreateNodeContextualMenu());
            
            this.AddManipulator(CreateGroupContextualMenu());
        }

        private IManipulator CreateGroupContextualMenu()
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction("Add Group", actionEvent => AddElement(CreateGroup("Node Group",actionEvent.eventInfo.localMousePosition))));

            return contextualMenuManipulator;
        }

        private Group CreateGroup(string title, Vector2 position)
        {
            Group group = new Group()
            {
                title = title
            };
            group.SetPosition(new Rect(position, Vector2.zero));
            return group;
        }

        protected virtual IManipulator CreateNodeContextualMenu()
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction("Add Node", actionEvent => AddElement(CreateNode<PMRNode>(actionEvent.eventInfo.localMousePosition))));

            return contextualMenuManipulator;
        }

        protected PMRNode CreateNode<TNode>(Vector2 position) where TNode : PMRNode, new()
        {
            TNode node = new TNode();
            
            node.Initialize(position);
            node.Draw();

            return node;
        }

        private void AddStyles()
        {
            this.AddStyleSheets(
                "PMRGraphView/PMRGraphViewStyles.uss",
                "PMRGraphView/PMRNodeStyles.uss");
        }

        private void AddGridBackground()
        {
            GridBackground gridBackground = new GridBackground();
            
            gridBackground.StretchToParentSize();
            
            Insert(0, gridBackground);
        }
    }
}
