using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PMR.Data
{
    using ScriptableObjects;
    [Serializable]
    public class PMRDialogueChoiceData
    {
        [field: SerializeField] public string Text { get; set; }
        [field: SerializeField] public PMRDIalogueSO NextDialogue { get; set; }
    }
}
