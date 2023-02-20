using System;
using System.Collections.Generic;
using PMR.GraphEditor.Save;
using PMR.GraphEditor.Utilities;
using PMR.ScriptableObjects;
using UnityEditor.Experimental.GraphView;

namespace PMR.GraphEditor.Elements
{
    public class PMRGroup : Group, IPMRSavedNode
    {
        [ReadOnly] public string ID;

        public PMRGroup()
        {
            ID = Guid.NewGuid().ToString();
        }

        public PMRNodeSaveData CreateEditorSaveData()
        {
            PMRGroupSaveData saveData = new PMRGroupSaveData()
            {
                ID = ID,
                Name = name,
                Position = GetPosition().position,
                GroupID = ""
            };
            return saveData;
        }

        public PMRGraphSO CreateRuntimeSaveData(string path, string fileName)
        {
            PMRGroupSO groupSO = PMRIOUtility.CreateAsset<PMRGroupSO>(path, fileName);
            groupSO.Initialize(fileName);
            return groupSO;
        }

        public void UpdateConnection(PMRGraphSO nodeSo, Dictionary<string, PMRGraphSO> createdNodes)
        {
            
        }
    }
}
