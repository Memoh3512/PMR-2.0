using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PMR.ScriptableObjects
{
    public class PMRGraphSO : ScriptableObject
    {
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public bool IsStartingNode { get; set; }

        public void Initialize(string newName, bool isStartingNode = false)
        {
            Name = newName;
            IsStartingNode = isStartingNode;
        }

        public virtual PMRGraphSO Execute()
        {
            Debug.LogWarning($"Executing a PMRGraphSO that has no Execute() method! {Name}");
            return null;
        }
    }
}
