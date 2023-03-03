using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PMR.GraphEditor.Save
{
    
    using Elements;
    
    [Serializable]
    public class PMRDialogueChoiceSaveData : PMRDialogueSaveData
    {
        [field: SerializeField] public List<PMRChoiceSaveData> Choices { get; set; }
        
        public override PMRNode LoadData(PMRGraphView graphView)
        {
            DialogueEditorChoiceNode node = graphView.CreateNode<DialogueEditorChoiceNode>(Name, Position, false);
            node.DialogueText = Text;
            node.ID = ID;
            node.Choices = node.CloneNodeChoices(Choices);
            
            return node;

        }
        
    }
}
