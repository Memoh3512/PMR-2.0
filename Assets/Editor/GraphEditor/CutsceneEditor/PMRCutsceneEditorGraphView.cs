using UnityEngine.UIElements;

namespace PMR.GraphEditor
{
    using Elements;
    public class PMRCutsceneEditorGraphView : PMRGraphView
    {
        public PMRCutsceneEditorGraphView(PMRGraphEditorWindow newEditorWindow) : base(newEditorWindow)
        {
            AddSearchWindow<PMRCutsceneEditorSearchWindow>();
        }
        
        protected override IManipulator CreateNodeContextualMenu()
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent =>
                {
                    //menuEvent.menu.AppendAction("Add Text Node", actionEvent => AddElement(CreateNode<DialogueEditorTextNode>("NewTextNode", actionEvent.eventInfo.localMousePosition)));
                    //menuEvent.menu.AppendAction("Add Choice Node", actionEvent => AddElement(CreateNode<DialogueEditorChoiceNode>("NewChoiceNode", actionEvent.eventInfo.localMousePosition)));
                });
            return contextualMenuManipulator;
        }
    }
}