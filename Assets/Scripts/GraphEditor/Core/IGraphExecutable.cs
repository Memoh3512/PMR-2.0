using System;

namespace PMR.ScriptableObjects
{
    public interface IGraphExecutable
    {
        public void Execute(GraphExecutionContext context, Action<GraphExecutionResult> finishedCallback);
    }
}