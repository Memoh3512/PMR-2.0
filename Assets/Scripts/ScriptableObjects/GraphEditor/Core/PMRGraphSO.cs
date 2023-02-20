using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PMR.ScriptableObjects
{
    public class PMRGraphSO : ScriptableObject
    {
        public string Name { get; set; }

        public void Initialize(string newName)
        {
            Name = newName;
        }
    }
}
