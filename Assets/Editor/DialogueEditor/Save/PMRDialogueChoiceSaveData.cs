using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PMR.GraphEditor.Save
{
    [Serializable]
    public class PMRDialogueChoiceSaveData : PMRDialogueSaveData
    {
        [field: SerializeField] public List<PMRChoiceSaveData> Choices { get; set; }
    }
}
