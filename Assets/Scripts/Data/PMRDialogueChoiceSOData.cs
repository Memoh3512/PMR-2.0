using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PMR.ScriptableObjects
{
    [Serializable]
    public class PMRDialogueChoiceSOData
    {
        [field: SerializeField] public string Text { get; set; }
        [field: SerializeField] public PMRGraphSO NextDialogue { get; set; }
    }
}
