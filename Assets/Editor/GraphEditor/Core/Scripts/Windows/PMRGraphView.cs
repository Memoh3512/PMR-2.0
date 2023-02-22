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
        private PMRGraphEditorWindow editorWindow;
        private PMRSearchWindow searchWindow;
        public PMRGraphView(PMRGraphEditorWindow newEditorWindow)
        {
            editorWindow = newEditorWindow;
            AddManipulators();
            AddSearchWindow();
            AddGridBackground();

            OnGroupElementsAdded();
            OnGroupElementsRemoved();
            OnGraphViewChanged();
            
            AddStyles();
        }
        private void AddSearchWindow()
        {
            if (searchWindow == null)
            {
                searchWindow = ScriptableObject.CreateInstance<PMRSearchWindow>();
                
                searchWindow.Initialize(this);
            }

            nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindow);
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
                menuEvent => menuEvent.menu.AppendAction("Add Group", actionEvent => AddElement(CreateGroup("Node Group",GetLocalMousePosition(actionEvent.eventInfo.localMousePosition)))));

            return contextualMenuManipulator;
        }
        private PMRGroup CreateGroup(string title, Vector2 position)
        {
            PMRGroup group = new PMRGroup()
            {
                title = title
            };

            foreach (GraphElement selectedElement in selection)
            {
                if (!(selectedElement is PMRNode)) continue;

                PMRNode node = (PMRNode)selectedElement;
                group.AddElement(node);
                node.Group = group;

            }
            group.SetPosition(new Rect(position, Vector2.zero));
            return group;
        }
        protected virtual IManipulator CreateNodeContextualMenu()
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction("Add Node", actionEvent => AddElement(CreateNode<PMRNode>(GetLocalMousePosition(actionEvent.eventInfo.localMousePosition)))));

            return contextualMenuManipulator;
        }
        public PMRNode CreateNode<TNode>(Vector2 position) where TNode : PMRNode, new()
        {
            TNode node = new TNode();
            
            node.Initialize(this, position);
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
        public Vector2 GetLocalMousePosition(Vector2 mousePosition, bool isSearchWindow = false)
        {
            Vector2 worldMousePosition = mousePosition;
            
            if (isSearchWindow)
            {
                worldMousePosition -= editorWindow.position.position;
            }
            
            Vector2 localMousePosition = contentViewContainer.WorldToLocal(worldMousePosition);
            return localMousePosition;
        }
        private void OnGraphViewChanged()
        {
            graphViewChanged = (changes) =>
            {
                if (changes.edgesToCreate != null)
                {
                    foreach (Edge edge in changes.edgesToCreate)
                    {
                        PMRPort fromPort = (PMRPort)edge.input;
                        PMRPort toPort = (PMRPort)edge.output;
                        fromPort.CallOnConnect(toPort);
                        toPort.CallOnConnect(fromPort);

                    }
                }

                if (changes.elementsToRemove != null)
                {
                    foreach (GraphElement element in changes.elementsToRemove)
                    {
                        if (!(element is Edge)) continue;

                        Edge edge = (Edge)element;
                        PMRPort fromPort = (PMRPort)edge.input;
                        PMRPort toPort = (PMRPort)edge.output;
                        fromPort.CallOnDisconnect(toPort);
                        toPort.CallOnDisconnect(fromPort);

                    }
                }

                return changes;

            };
        }
        private void OnGroupElementsAdded()
        {
            elementsAddedToGroup = (group, elements) =>
            {
                foreach (GraphElement element in elements)
                {
                    if (!(element is PMRNode))
                    {
                        continue;
                    }

                    PMRGroup pmrGroup = (PMRGroup) group;
                    PMRNode node = (PMRNode) element;

                    node.Group = pmrGroup;
                    //Debug.Log($"Set group to grouped node! {node.NodeName} inside {pmrGroup.title}");
                }
            };
        }
        private void OnGroupElementsRemoved()
        {
            elementsRemovedFromGroup = (group, elements) =>
            {
                foreach (GraphElement element in elements)
                {
                    if (!(element is PMRNode))
                    {
                        continue;
                    }

                    PMRGroup pmrGroup = (PMRGroup) group;
                    PMRNode node = (PMRNode) element;

                    node.Group = null;
                }
            };
        }

        public void ClearGraph()
        {
            graphElements.ForEach(RemoveElement);
        }
        
    }
}
