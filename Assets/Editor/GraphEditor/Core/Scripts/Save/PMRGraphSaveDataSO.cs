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
        public List<string> OldGroupNames { get; set; }
        public List<string> OldNodeNames { get; set; }
        public SerializableDictionary<string, List<string>> OldGroupedNodeNames { get; set; }

        public void Initialize(string filename)
        {
            FileName = filename;

            Groups = new List<PMRGroupSaveData>();
            Nodes = new List<PMRNodeSaveData>();
        }
        
    }
}
