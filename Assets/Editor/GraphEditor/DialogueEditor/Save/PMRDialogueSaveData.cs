using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PMR.GraphEditor.Save
{
    
    using Elements;
    
    public class PMRDialogueSaveData : PMRNodeSaveData
    {
        [field: SerializeField] public string Text { get; set; }
        [field: SerializeField] public string NextNodeID { get; set; }
        
        public override PMRNode LoadData(PMRGraphView graphView)
        {
            DialogueEditorTextNode node = graphView.CreateNode<DialogueEditorTextNode>(Name, Position, false);
            node.DialogueText = Text;
            node.ID = ID;
            node.NextNodeID = NextNodeID;
            
            return node;

        }
    }
}
