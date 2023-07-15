namespace PMR.ScriptableObjects
{
    public interface IGraphExecutable
    {
        public GraphExecutionResult Execute(GraphExecutionContext context);
    }
}