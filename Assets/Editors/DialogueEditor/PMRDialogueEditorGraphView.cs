using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace PMR.GraphEditor
{
    using Elements;
    public class PMRDialogueEditorGraphView : PMRGraphView
    {

        protected override IManipulator CreateNodeContextualMenu()
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent =>
                {
                    menuEvent.menu.AppendAction("Add Text Node", actionEvent => AddElement(CreateNode<DialogueEditorTextNode>(actionEvent.eventInfo.localMousePosition)));
                    menuEvent.menu.AppendAction("Add Choice Node", actionEvent => AddElement(CreateNode<DialogueEditorChoiceNode>(actionEvent.eventInfo.localMousePosition)));
                });

            return contextualMenuManipulator;
        }
    }
}
