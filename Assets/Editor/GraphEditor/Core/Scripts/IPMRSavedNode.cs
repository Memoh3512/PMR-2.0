


using System.Collections.Generic;

namespace PMR.GraphEditor.Utilities
{
    using Save;
    using ScriptableObjects;
    
    public interface IPMRSavedNode
    {
        public PMRNodeSaveData CreateEditorSaveData();

        public PMRGraphSO CreateRuntimeSaveData(string path, string fileName);

        public void UpdateConnection(PMRGraphSO nodeSo, Dictionary<string, PMRGraphSO> createdNodes);
    }
}