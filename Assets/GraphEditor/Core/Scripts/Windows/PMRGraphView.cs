using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace PMR.GraphEditor
{
    
    using Elements;
    public class PMRGraphView : GraphView
    {
        public PMRGraphView()
        {
            AddManipulators();
            AddGridBackground();

            CreateNode();

            AddStyles();
        }

        private void CreateNode()
        {
            PMRNode node = new PMRNode();
            
            node.Initialize();
            node.Draw();
            
            AddElement(node);
        }

        private void AddManipulators()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            this.AddManipulator(new ContentDragger());
        }

        private void AddStyles()
        {
            StyleSheet stylesheet = (StyleSheet) EditorGUIUtility.Load("PMRGraphView/PMRGraphViewStyles.uss");
            
            styleSheets.Add(stylesheet);
        }

        private void AddGridBackground()
        {
            GridBackground gridBackground = new GridBackground();
            
            gridBackground.StretchToParentSize();
            
            Insert(0, gridBackground);
        }
    }
}
