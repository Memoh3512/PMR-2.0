using System;
using System.Collections.Generic;
using PMR.GraphEditor.Save;
using PMR.GraphEditor.Utilities;
using PMR.ScriptableObjects;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace PMR.GraphEditor.Elements
{
    public class PMRGroup : Group
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
                Name = title,
                Position = GetPosition().position.ToPMRVector2(),
                GroupID = ""
            };
            return saveData;
        }

        public PMRGraphSO CreateRuntimeSaveData(string path, string fileName)
        {
            PMRGroupSO groupSO = PMRIOUtility.CreateAsset<PMRGroupSO>(path, fileName);
            groupSO.Initialize(title);
            return groupSO;
        }
    }
}
