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
        
    }
}
