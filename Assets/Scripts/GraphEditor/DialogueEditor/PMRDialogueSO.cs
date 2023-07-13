using UnityEngine;

namespace PMR.ScriptableObjects
{
    public class PMRDialogueSO : PMRGraphSO, IDialogueExecutable
    {
        [field: SerializeField] [field: TextArea()] public string Text { get; set; }
        
        [field: SerializeField] public PMRGraphSO NextNode { get; set; }
        public override PMRGraphSO Execute()
        {
            
            return NextNode;
        }

        public DialogueExecutionResult Execute(DialogueExecutionContext context)
        {
            context.DialoguePlayer.TriggerText(Text);
            
            DialogueExecutionResult result = new DialogueExecutionResult(DialogueExecutionStatus.Wait);
            return result;
        }
    }
}
