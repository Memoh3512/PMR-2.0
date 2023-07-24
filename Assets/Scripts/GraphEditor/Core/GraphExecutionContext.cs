using UnityEngine;
using UnityEngine.Assertions;

namespace PMR.ScriptableObjects
{
    public class GraphExecutionContext
    {
        public DialoguePlayer DialoguePlayer { get; }
        public GameObject Source { get; }
        public GameObject Target { get; }

        public GraphExecutionContext(DialoguePlayer dialoguePlayer)
        {
            DialoguePlayer = dialoguePlayer;
        }

        public GraphExecutionContext(DialoguePlayer dialoguePlayer, GameObject source, GameObject target)
        {
            DialoguePlayer = dialoguePlayer;
            Source = source;
            Target = target;
        }
    }

    public class GraphExecutionResult
    {
        public GraphExecutionStatus Status { get; set; }
        public PMRGraphSO NextNode { get; set; }

        public GraphExecutionResult(GraphExecutionStatus status, PMRGraphSO nextNode)
        {
            Status = status;
            NextNode = nextNode;
            
            Assert.IsTrue(status != GraphExecutionStatus.Continue || nextNode != null, 
                $"Execution status set to \"Continue\" but next node is null! {GetType()}");
        }
        public GraphExecutionResult(GraphExecutionStatus status)
        {
            Status = status;
            
            Assert.IsTrue(status != GraphExecutionStatus.Continue, 
                $"Execution status set to \"Continue\" but next node is null! {GetType()}");
        }
    }

    public enum GraphExecutionStatus
    {
        Continue,
        Stop,
        Wait
    }
}