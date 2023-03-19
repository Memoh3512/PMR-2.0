using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PMR.ScriptableObjects
{
    public class PMRGraphSO : ScriptableObject
    {
        [field: SerializeField] public string Name { get; set; }

        public void Initialize(string newName)
        {
            Name = newName;
        }
    }
}
