using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PMR.ScriptableObjects
{
    public class PMRContainerSO : ScriptableObject
    {
        [field: SerializeField]public string FileName { get; set; }
        [field: SerializeField]public SerializableDictionary<PMRGroupSO, List<PMRGraphSO>> Groups { get; set; }
        [field: SerializeField]public List<PMRGraphSO> UngroupedNodes { get; set; }

        public void Initialize(string fileName)
        {
            FileName = fileName;
            Groups = new SerializableDictionary<PMRGroupSO, List<PMRGraphSO>>();
            UngroupedNodes = new List<PMRGraphSO>();
        }

        public List<PMRGroupSO> GetGroups()
        {
            List<PMRGroupSO> groups = new List<PMRGroupSO>();
            groups.AddRange(Groups.Keys);
            return groups;
        }
        public List<string> GetGroupNames()
        {
            return GetNodeNames(Groups.Keys);
        }

        public List<PMRGraphSO> GetGroupedNodes(PMRGroupSO group, bool startingNodesOnly, params Type[] types)
        {
            if (types == null || types.Length == 0)
            {
                return Groups[group];
            }
            
            List<PMRGraphSO> groupedNodes = new List<PMRGraphSO>();
            foreach (PMRGraphSO node in Groups[group])
            {
                if (startingNodesOnly && !node.IsStartingNode) continue;
                
                Type nodeType = node.GetType();
                foreach (Type t in types)
                {
                    if (nodeType == t)
                    {
                        groupedNodes.Add(node);
                        break;
                    }
                }
            }
            return groupedNodes;
        }
        
        public List<string> GetGroupedNodeNames(PMRGroupSO group, bool startingNodesOnly, params Type[] types)
        {
            return GetNodeNames(GetGroupedNodes(group, startingNodesOnly, types));
        }
        
        public List<PMRGraphSO> GetUngroupedNodes(bool startingNodesOnly, params Type[] types)
        {
            if (types == null || types.Length == 0)
            {
                return UngroupedNodes;
            }
            
            List<PMRGraphSO> ungroupedNodesResult = new List<PMRGraphSO>();
            foreach (PMRGraphSO node in UngroupedNodes)
            {
                if (startingNodesOnly && !node.IsStartingNode) continue;
                
                Type nodeType = node.GetType();
                foreach (Type t in types)
                {
                    if (nodeType == t)
                    {
                        ungroupedNodesResult.Add(node);
                        break;
                    }
                }
            }
            return ungroupedNodesResult;
        }

        public List<string> GetUngroupedNodeNames(bool startingNodesOnly, params Type[] types)
        {
            return GetNodeNames(GetUngroupedNodes(startingNodesOnly, types));
        }

        private List<string> GetNodeNames(IEnumerable<PMRGraphSO> nodes)
        {
            List<string> nodeNames = new List<string>();
            foreach (PMRGraphSO node in nodes)
            {
                nodeNames.Add(node.Name);
            }
            return nodeNames;
        }

    }
}
