using System;
using UnityEngine;

namespace PMR.ScriptableObjects
{
    public class PMRDialogueSO : PMRGraphSO
    {
        [field: SerializeField] [field: TextArea()] public string Text { get; set; }
        [field: SerializeField] public PMRGraphSO NextNode { get; set; }

        public override void Execute(GraphExecutionContext context, Action<GraphExecutionResult> finishedCallback)
        {
            context.DialoguePlayer.TriggerText(Text);

            GraphExecutionStatus status = GraphExecutionStatus.Wait;
            finishedCallback(new GraphExecutionResult(status, NextNode));
        }
    }
}
