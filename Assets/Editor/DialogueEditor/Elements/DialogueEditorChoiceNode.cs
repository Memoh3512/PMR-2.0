using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace PMR
{
    using GraphEditor;
    using GraphEditor.Elements;
    using GraphEditor.Utilities;
    using GraphEditor.Save;
    public class DialogueEditorChoiceNode : PMRNode
    {
        public  string DialogueText { get; set; }
        public List<PMRDialogueChoiceSaveData> Choices { get; set; }
        public override void Initialize(PMRGraphView pmrGraphView, Vector2 position)
        {
            base.Initialize(pmrGraphView, position);
            NodeName = "Choice";
            Choices = new List<PMRDialogueChoiceSaveData>();

            PMRDialogueChoiceSaveData choiceData = new PMRDialogueChoiceSaveData()
            {
                Text = "New Choice"
            };
            
            Choices.Add(choiceData);

        }

        public override void Draw()
        {
            base.Draw();

            Button addChoiceButton = PMRElementUtility.CreateButton("Add Choice", () =>
            {
                PMRDialogueChoiceSaveData choiceData = new PMRDialogueChoiceSaveData()
                {
                    Text = "New Choice"
                };
                Choices.Add(choiceData);
             
                PMRPort choicePort = CreateChoicePort(choiceData);
                outputContainer.Add(choicePort);
            });
            addChoiceButton.AddToClassList("ds-node__button");
            
            mainContainer.Insert(1, addChoiceButton);
            
            //input
            PMRPort inputPort = this.CreatePort("Input", Direction.Input, Port.Capacity.Multi);
            inputContainer.Add(inputPort);
            
            //choices output
            foreach (PMRDialogueChoiceSaveData choice in Choices)
            {
                PMRPort choicePort = CreateChoicePort(choice);
                outputContainer.Add(choicePort);
            }
            
            //text
            VisualElement customDataContainer = new VisualElement();
            customDataContainer.AddToClassList("ds-node__custom-data-container");
            
            Foldout textFoldout = PMRElementUtility.CreateFoldout("Dialogue Text");

            TextField textTextField = PMRElementUtility.CreateTextArea(DialogueText);
            textTextField.AddClasses(
                "ds-node__text-field",
                "ds-node__quote-text-field");

            textFoldout.Add(textTextField);
            customDataContainer.Add(textFoldout);
            
            extensionContainer.Add(customDataContainer);
            
            RefreshExpandedState();
            
        }

        private PMRPort CreateChoicePort(PMRDialogueChoiceSaveData userData)
        {
            PMRPort choicePort = this.CreatePort();
            choicePort.userData = userData;

            choicePort.OnConnect = (other) =>
            {
                userData.NodeID = ((PMRNode)other.node).ID;
            };
            
            choicePort.OnDisconnect = (other) =>
            {
                userData.NodeID = "";

            };

            Button deleteChoiceButton = PMRElementUtility.CreateButton("X", () =>
            {
                if (Choices.Count == 1) return;

                if (choicePort.connected)
                {
                    graphView.DeleteElements(choicePort.connections);
                }

                Choices.Remove(userData);
                graphView.RemoveElement(choicePort);
            });
            deleteChoiceButton.AddToClassList("ds-node__button");
                
            TextField choiceTextField = PMRElementUtility.CreateTextField(userData.Text, null, callback =>
            {
                userData.Text = callback.newValue;
            });
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
