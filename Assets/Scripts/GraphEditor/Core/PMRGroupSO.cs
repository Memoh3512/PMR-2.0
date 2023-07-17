using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace PMR.ScriptableObjects
{
    public class PMRGroupSO : PMRGraphSO
    {
        public override void Execute(GraphExecutionContext context, Action<GraphExecutionResult> finishedCallback)
        {
            Assert.IsTrue(false, "Trying to execute a node group! This should never happen.");
        }
    }
}
