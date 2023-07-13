namespace PMR.ScriptableObjects
{
    public interface IDialogueExecutable
    {
        public DialogueExecutionResult Execute(DialogueExecutionContext context);
    }
}