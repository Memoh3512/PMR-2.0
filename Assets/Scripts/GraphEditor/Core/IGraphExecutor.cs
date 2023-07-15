namespace PMR.ScriptableObjects
{
    public interface IGraphExecutor
    {
        public void ExecuteDialogueNode(IGraphExecutable node, GraphExecutionContext context);
    }
}