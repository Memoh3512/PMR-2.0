using System.Collections;
using System.Collections.Generic;
using PMR.GraphEditor.Elements;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace PMR
{
    using GraphEditor.Utilities;
    public class DialogueEditorChoiceNode : PMRNode
    {
        private string dialogueText;
        private List<string> choices = new List<string>();
        public override void Initialize(Vector2 position)
        {
            base.Initialize(position);
            NodeName = "Choice";
            
            choices.Add("New Choice");
        }

        public override void Draw()
        {
            base.Draw();

            Button addChoiceButton = PMRElementUtility.CreateButton("Add Choice", () =>
            {
                Port choicePort = CreateChoicePort("New Choice");
                
                choices.Add("New Choice");
                
                outputContainer.Add(choicePort);
            });
            addChoiceButton.AddToClassList("ds-node__button");
            
            mainContainer.Insert(1, addChoiceButton);
            
            //input
            Port inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
            inputPort.portName = "Input";

            inputContainer.Add(inputPort);
            
            //choices output
            foreach (string choice in choices)
            {
                Port choicePort = CreateChoicePort(choice);
                outputContainer.Add(choicePort);
            }
            
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

        private Port CreateChoicePort(string choice)
        {
            Port choicePort = this.CreatePort();
            choicePort.portName = "";

            Button deleteChoiceButton = PMRElementUtility.CreateButton("X");
            deleteChoiceButton.AddToClassList("ds-node__button");
                
            TextField choiceTextField = PMRElementUtility.CreateTextField(choice);
            choiceTextField.AddClasses(
                "ds-node__text-field",
                "ds-node__choice-text-field",
                "ds-node__text-field__hidden");
                
            choicePort.Add(choiceTextField);
            choicePort.Add(deleteChoiceButton);
            
            return choicePort;
        }
    }
}
