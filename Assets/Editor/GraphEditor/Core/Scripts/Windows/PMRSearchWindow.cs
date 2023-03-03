using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace PMR.GraphEditor
{
    using Elements;
    public class PMRSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private PMRGraphView graphView;
        private Texture2D indentationIcon;
        public void Initialize(PMRGraphView newGraphView)
        {
            graphView = newGraphView;

            indentationIcon = new Texture2D(1, 1);
            indentationIcon.SetPixel(0,0, Color.clear);
            indentationIcon.Apply();
        }
        
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> searchTreeEntries = new List<SearchTreeEntry>()
            {
                new SearchTreeGroupEntry(new GUIContent("Create Element")),
                new SearchTreeGroupEntry(new GUIContent("Dialogue Node"), 1),
                new SearchTreeEntry(new GUIContent("Text", indentationIcon))
                {
                    level = 2,
                    userData = typeof(DialogueEditorTextNode)
                },
                new SearchTreeEntry(new GUIContent("Choice", indentationIcon))
                {
                    level = 2,
                    userData = typeof(DialogueEditorChoiceNode)
                },
                new SearchTreeGroupEntry(new GUIContent("Group"), 1),
                new SearchTreeEntry(new GUIContent("Single Group", indentationIcon))
                {
                    level = 2,
                    userData = typeof(PMRGroup)
                }
            };

            return searchTreeEntries;

        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            if (SearchTreeEntry.userData != null)
            {
                Vector2 localMousePosition = graphView.GetLocalMousePosition(context.screenMousePosition, true);
                
                //sus cast but should work as long as we put visualelements in entries
                GraphElement element = (GraphElement)Activator.CreateInstance((Type)SearchTreeEntry.userData);
                if (element.GetType().IsSubclassOf(typeof(PMRNode)))
                {
                    ((PMRNode)element).Initialize("NewNode", graphView, localMousePosition);
                    ((PMRNode)element).Draw();
                } else if (element.GetType() == typeof(PMRGroup))
                {
                    ((PMRGroup)element).title = "Node Group";
                    ((PMRGroup)element).SetPosition(new Rect(localMousePosition, Vector2.zero));
                }
                graphView?.AddElement(element);
                return true;
            }

            return false;
        }
    }
}
