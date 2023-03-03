using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace PMR.GraphEditor
{
    using Elements;
    public class PMRDialogueEditorGraphView : PMRGraphView
    {

        public PMRDialogueEditorGraphView(PMRGraphEditorWindow newEditorWindow) : base(newEditorWindow) { }
        
        protected override IManipulator CreateNodeContextualMenu()
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent =>
                {
                    menuEvent.menu.AppendAction("Add Text Node", actionEvent => AddElement(CreateNode<DialogueEditorTextNode>("NewTextNode", actionEvent.eventInfo.localMousePosition)));
                    menuEvent.menu.AppendAction("Add Choice Node", actionEvent => AddElement(CreateNode<DialogueEditorChoiceNode>("NewChoiceNode", actionEvent.eventInfo.localMousePosition)));
                });

            return contextualMenuManipulator;
        }
    }
}
