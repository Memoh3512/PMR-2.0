using UnityEngine;
using UnityEngine.Assertions;

namespace PMR.ScriptableObjects
{
    public class DialogueExecutionContext
    {
        public DialoguePlayer DialoguePlayer { get; }
        public GameObject Source { get; }
        public GameObject Target { get; }
        
        public DialogueExecutionContext(DialoguePlayer dialoguePlayer)
        {
            DialoguePlayer = dialoguePlayer;
        }

        public DialogueExecutionContext(DialoguePlayer dialoguePlayer, GameObject source, GameObject target)
        {
            DialoguePlayer = dialoguePlayer;
            Source = source;
            Target = target;
        }
    }

    public class DialogueExecutionResult
    {
        public DialogueExecutionStatus Status { get; set; }
        public PMRGraphSO NextNode { get; set; }

        public DialogueExecutionResult(DialogueExecutionStatus status, PMRGraphSO nextNode)
        {
            Status = status;
            NextNode = nextNode;
            
            Assert.IsTrue(status != DialogueExecutionStatus.Continue || nextNode != null, 
                $"Execution status set to \"Continue\" but next node is null! {GetType()}");
        }
        public DialogueExecutionResult(DialogueExecutionStatus status)
        {
            Status = status;
            
            Assert.IsTrue(status != DialogueExecutionStatus.Continue, 
                $"Execution status set to \"Continue\" but next node is null! {GetType()}");
        }
    }

    public enum DialogueExecutionStatus
    {
        Continue,
        Stop,
        Wait
    }
}