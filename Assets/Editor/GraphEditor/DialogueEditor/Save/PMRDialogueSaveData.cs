using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PMR.GraphEditor.Save
{
    public class PMRDialogueSaveData : PMRNodeSaveData
    {
        [field: SerializeField] public string Text { get; set; }
    }
}
