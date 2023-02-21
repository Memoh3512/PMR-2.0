using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PMR.GraphEditor.Save
{
    public class PMRGraphSaveDataSO : ScriptableObject
    {
        [field: SerializeField] public string FileName { get; set; }
        [field: SerializeField] public List<PMRGroupSaveData> Groups { get; set; }
        [field: SerializeField] public List<PMRNodeSaveData> Nodes { get; set; }
        [field: SerializeField] public List<string> OldGroupIDs { get; set; }
        [field: SerializeField] public List<string> OldNodeIDs { get; set; }
        [field: SerializeField] public SerializableDictionary<string, List<string>> OldGroupedNodeIDs { get; set; }

        public void Initialize(string filename)
        {
            FileName = filename;

            Groups = new List<PMRGroupSaveData>();
            Nodes = new List<PMRNodeSaveData>();
        }
        
    }
}
