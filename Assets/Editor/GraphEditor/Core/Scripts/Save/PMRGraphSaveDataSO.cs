using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PMR.GraphEditor.Save
{
    public class PMRGraphSaveDataSO : ScriptableObject
    {
        public string FileName { get; set; }
        public List<PMRGroupSaveData> Groups { get; set; }
        public List<PMRNodeSaveData> Nodes { get; set; }
        public List<string> OldGroupIDs { get; set; }
        public List<string> OldNodeIDs { get; set; }
        public SerializableDictionary<string, List<string>> OldGroupedNodeIDs { get; set; }

        public void Initialize(string filename)
        {
            FileName = filename;

            Groups = new List<PMRGroupSaveData>();
            Nodes = new List<PMRNodeSaveData>();
        }
        
    }
}
