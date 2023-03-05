using System.Collections.Generic;
using System.Linq;
using PMR.GraphEditor.Save;
using PMR.ScriptableObjects;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace PMR.GraphEditor.Elements
{
    using Utilities;
    public class DialogueEditorTextNode : PMRNode
    {
        public string DialogueText { get; set; }
        
        public string NextNodeID { get; set; }
        public override void Initialize(string nodeName, PMRGraphView pmrGraphView, Vector2 position)
        {
            base.Initialize(nodeName, pmrGraphView, position);
            NodeName = nodeName;
        }

        public override void Draw()
        {
            base.Draw();

            PMRPort inputPort = this.CreatePort("Dialogue Connection", Direction.Input, Port.Capacity.Multi);
            inputContainer.Add(inputPort);
            
            PMRPort outputPort = this.CreatePort("Next dialogue");
            outputContainer.Add(outputPort);

            outputPort.OnConnect = (other) =>
            {
                NextNodeID = ((PMRNode)other.node).ID;
            };
            outputPort.OnDisconnect = (other) =>
            {
                NextNodeID = "";
            };

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

        public override PMRNodeSaveData CreateEditorSaveData()
        {
            PMRDialogueSaveData saveData = new PMRDialogueSaveData()
            {
                ID = ID,
                GroupID = Group?.ID,
                Name = NodeName,
                Position = GetPosition().position,
                NextNodeID = NextNodeID,
                Text = DialogueText
            };
            return saveData;
        }

        public override PMRGraphSO CreateRuntimeSaveData(string path, string fileName)
        {
            PMRDialogueSO dialogueSO = PMRIOUtility.CreateAsset<PMRDialogueSO>(path, fileName);
            dialogueSO.Initialize(NodeName);
            dialogueSO.Text = DialogueText;
            dialogueSO.IsStartingDialogue = !((PMRPort) inputContainer.Children().First()).connected;
            
            return dialogueSO;
        }

        public override void SaveConnections(PMRGraphSO nodeSo, Dictionary<string, PMRGraphSO> createdNodes)
        {
            PMRDialogueSO dialogueSO = (PMRDialogueSO)nodeSo;
            if (string.IsNullOrEmpty(NextNodeID)) return; //validation d'un coup qu'on delete une connection de maniere bizarre
            if (createdNodes.ContainsKey(NextNodeID))
            {
                dialogueSO.NextNode = createdNodes[NextNodeID];
            }
        }

        public override void LoadConnections(Dictionary<string, PMRNode> loadedNodes)
        {
            PMRNode nextNode = loadedNodes[NextNodeID];
            PMRPort nextNodeInput = (PMRPort)nextNode.inputContainer.Children().First();
            PMRPort output = (PMRPort)outputContainer.Children().First();

            Edge edge = output.ConnectTo(nextNodeInput);
            graphView.AddElement(edge);
        }
        
    }
}