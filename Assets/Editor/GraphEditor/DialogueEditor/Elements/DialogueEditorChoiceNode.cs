using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PMR.ScriptableObjects;
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
        public string DialogueText { get; set; }
        public List<PMRChoiceSaveData> Choices { get; set; }
        public override void Initialize(string nodeName, PMRGraphView pmrGraphView, Vector2 position)
        {
            base.Initialize(nodeName, pmrGraphView, position);
            NodeName = nodeName;
            Choices = new List<PMRChoiceSaveData>();

            PMRChoiceSaveData choiceData = new PMRChoiceSaveData()
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
                PMRChoiceSaveData choiceData = new PMRChoiceSaveData()
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
            foreach (PMRChoiceSaveData choice in Choices)
            {
                PMRPort choicePort = CreateChoicePort(choice);
                outputContainer.Add(choicePort);
            }
            
            //text
            VisualElement customDataContainer = new VisualElement();
            customDataContainer.AddToClassList("ds-node__custom-data-container");
            
            Foldout textFoldout = PMRElementUtility.CreateFoldout("Dialogue Text");

            TextField textTextField = PMRElementUtility.CreateTextArea(DialogueText, null, callback =>
            {
                DialogueText = callback.newValue;
            });
            textTextField.AddClasses(
                "ds-node__text-field",
                "ds-node__quote-text-field");

            textFoldout.Add(textTextField);
            customDataContainer.Add(textFoldout);
            
            extensionContainer.Add(customDataContainer);
            
            RefreshExpandedState();
            
        }

        private PMRPort CreateChoicePort(PMRChoiceSaveData userData)
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

        public override PMRNodeSaveData CreateEditorSaveData()
        {

            List<PMRChoiceSaveData> savedChoices = CloneNodeChoices(Choices);
            
            PMRDialogueChoiceSaveData saveData = new PMRDialogueChoiceSaveData()
            {
                ID = ID,
                GroupID = Group?.ID,
                Name = NodeName,
                Position = GetPosition().position,
                Text = DialogueText,
                Choices = savedChoices
            };
            return saveData;
        }

        public List<PMRChoiceSaveData> CloneNodeChoices(List<PMRChoiceSaveData> choices)
        {
            List<PMRChoiceSaveData> clonedChoices = new List<PMRChoiceSaveData>();

            foreach (PMRChoiceSaveData choice in choices)
            {
                PMRChoiceSaveData newChoice = new PMRChoiceSaveData()
                {
                    Text = choice.Text,
                    NodeID = choice.NodeID
                };
                clonedChoices.Add(newChoice);
            }

            return clonedChoices;
        }

        public override PMRGraphSO CreateRuntimeSaveData(string path, string fileName)
        {
            PMRDialogueChoiceSO dialogueChoiceSO = PMRIOUtility.CreateAsset<PMRDialogueChoiceSO>(path, fileName);
            dialogueChoiceSO.Initialize(NodeName);
            dialogueChoiceSO.Text = DialogueText;
            dialogueChoiceSO.IsStartingDialogue = !((PMRPort) inputContainer.Children().First()).connected;
            dialogueChoiceSO.Choices = ConvertEditorToRuntimeChoices();
            
            return dialogueChoiceSO;
        }

        public override void SaveConnections(PMRGraphSO nodeSo, Dictionary<string, PMRGraphSO> createdNodes)
        {
            PMRDialogueChoiceSO dialogueChoiceSO = (PMRDialogueChoiceSO)nodeSo;
            if (dialogueChoiceSO == null)
            {
                Debug.Log("DIALOGUECHOICESO NULL!! WTF UPDATECONNECTION Choice node");
                return;
            }

            if (dialogueChoiceSO.Choices == null)
            {                
                Debug.Log("dialogueChoiceSO.Choices NULL!! WTF UPDATECONNECTION Choice node");
                return;
            }
            for (int i = 0; i < dialogueChoiceSO.Choices.Count; i++)
            {
                PMRChoiceSaveData choice = Choices[i];
                
                if (string.IsNullOrEmpty(choice.NodeID)) continue;

                dialogueChoiceSO.Choices[i].NextDialogue = createdNodes[choice.NodeID];

            }
        }
        
        public override void LoadConnections(Dictionary<string, PMRNode> loadedNodes)
        {
            foreach (VisualElement outputItem in outputContainer.Children())
            {
                if (outputItem is PMRPort choicePort)
                {
                    PMRChoiceSaveData choiceData = (PMRChoiceSaveData)choicePort.userData;

                    if (string.IsNullOrEmpty(choiceData.NodeID)) continue;
                    
                    PMRNode nextNode = loadedNodes[choiceData.NodeID];
                    PMRPort nextNodeInput = (PMRPort)nextNode.inputContainer.Children().First();

                    Edge edge = choicePort.ConnectTo(nextNodeInput);
                    graphView.AddElement(edge);  

                }
            }
        }
        
        private List<PMRDialogueChoiceSOData> ConvertEditorToRuntimeChoices()
        {
            List<PMRDialogueChoiceSOData> convertedChoices = new List<PMRDialogueChoiceSOData>();

            foreach (PMRChoiceSaveData nodeChoice in Choices)
            {
                PMRDialogueChoiceSOData data = new PMRDialogueChoiceSOData()
                {
                    Text = nodeChoice.Text
                };
                convertedChoices.Add(data);
            }

            return convertedChoices;
        }
    }
}
