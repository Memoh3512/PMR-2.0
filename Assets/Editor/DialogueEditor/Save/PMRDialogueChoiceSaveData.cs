using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PMR.GraphEditor.Save
{
    [Serializable]
    public class PMRDialogueChoiceSaveData
    {
        [field: SerializeField] public string Text { get; set; }
        [field: SerializeField] public string NodeID { get; set; }
    }
}
