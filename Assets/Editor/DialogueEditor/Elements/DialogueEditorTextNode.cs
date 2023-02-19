using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace PMR.GraphEditor.Elements
{
    using Utilities;
    public class DialogueEditorTextNode : PMRNode
    {
        private string dialogueText;
        public override void Initialize(PMRGraphView pmrGraphView, Vector2 position)
        {
            base.Initialize(pmrGraphView, position);
            NodeName = "Dialogue Text";
        }

        public override void Draw()
        {
            base.Draw();

            PMRPort inputPort = this.CreatePort("Dialogue Connection", Direction.Input, Port.Capacity.Multi);
            inputContainer.Add(inputPort);
            
            PMRPort outputPort = this.CreatePort("Next dialogue");
            outputContainer.Add(outputPort);

            //text
            VisualElement customDataContainer = new VisualElement();
            customDataContainer.AddToClassList("ds-node__custom-data-container");
            
            Foldout textFoldout = PMRElementUtility.CreateFoldout("Dialogue Text");

            TextField textTextField = PMRElementUtility.CreateTextArea(dialogueText);
            textTextField.AddClasses(
                "ds-node__text-field",
                "ds-node__quote-text-field");

            textFoldout.Add(textTextField);
            customDataContainer.Add(textFoldout);
            
            extensionContainer.Add(customDataContainer);
            
            RefreshExpandedState();
        }
    }
}