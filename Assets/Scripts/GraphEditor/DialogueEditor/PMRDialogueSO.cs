using UnityEngine;

namespace PMR.ScriptableObjects
{
    public class PMRDialogueSO : PMRGraphSO
    {
        [field: SerializeField] [field: TextArea()] public string Text { get; set; }
        
        [field: SerializeField] public PMRGraphSO NextNode { get; set; }

        public override GraphExecutionResult Execute(GraphExecutionContext context)
        {
            context.DialoguePlayer.TriggerText(Text);

            GraphExecutionStatus status = NextNode == null ? GraphExecutionStatus.Stop : GraphExecutionStatus.Wait;
            return new GraphExecutionResult(status, NextNode);
        }
    }
}
