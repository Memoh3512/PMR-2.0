using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PMR.ScriptableObjects
{
    public class PMRGraphSO : ScriptableObject
    {
        public string DialogueName { get; set; }

        public virtual void Initialize(string dialogueName)
        {
            DialogueName = dialogueName;
        }
    }
}
