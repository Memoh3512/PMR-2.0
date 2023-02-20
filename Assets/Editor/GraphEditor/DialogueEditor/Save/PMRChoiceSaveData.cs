using UnityEngine;

namespace PMR.GraphEditor.Save
{
    public class PMRChoiceSaveData
    {
        [field: SerializeField] public string Text { get; set; }
        [field: SerializeField] public string NodeID { get; set; }
    }
}