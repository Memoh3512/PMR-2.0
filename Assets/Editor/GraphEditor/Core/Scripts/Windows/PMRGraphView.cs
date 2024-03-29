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
        private MiniMap miniMap;
        public PMRGraphView(PMRGraphEditorWindow newEditorWindow)
        {
            editorWindow = newEditorWindow;
            AddManipulators();
            AddMinimap();
            AddGridBackground();

            OnGroupElementsAdded();
            OnGroupElementsRemoved();
            OnGraphViewChanged();
            
            AddStyles();
            AddMinimapStyles();
        }

        private void AddMinimap()
        {
            miniMap = new MiniMap()
            {
                anchored = true,
                visible = false
            };
            
            miniMap.SetPosition(new Rect(15,50,200,180));

            Add(miniMap);
        }

        protected void AddSearchWindow<T>() where T : PMRSearchWindow
        {
            if (searchWindow == null)
            {
                searchWindow = ScriptableObject.CreateInstance<T>();
                
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
                menuEvent => menuEvent.menu.AppendAction("Add Group", actionEvent => CreateGroup("Node Group",GetLocalMousePosition(actionEvent.eventInfo.localMousePosition))));

            return contextualMenuManipulator;
        }
        public PMRGroup CreateGroup(string title, Vector2 position)
        {
            PMRGroup group = new PMRGroup()
            {
                title = title
            };
            
            AddElement(group);

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
            return null;
        }
        public TNode CreateNode<TNode>(string nodeName, Vector2 position, bool shouldDraw = true) where TNode : PMRNode, new()
        {
            TNode node = new TNode();
            
            node.Initialize(nodeName, this, position);
            if (shouldDraw) node.Draw();

            return node;
        }
        private void AddStyles()
        {
            this.AddStyleSheets(
                "PMRGraphView/PMRGraphViewStyles.uss",
                "PMRGraphView/PMRNodeStyles.uss");
        }
        private void AddMinimapStyles()
        {
            StyleColor backgroundColor = new StyleColor(new Color32(29, 29, 30, 255));
            StyleColor borderColor = new StyleColor(new Color32(51, 51, 51, 255));

            miniMap.style.backgroundColor = backgroundColor;
            miniMap.style.borderTopColor = borderColor;
            miniMap.style.borderRightColor = borderColor;
            miniMap.style.borderBottomColor = borderColor;
            miniMap.style.borderLeftColor = borderColor;
        }

        public void ToggleMinimap()
        {
            miniMap.visible = !miniMap.visible;
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
