using UnityEngine;

namespace PMR.ScriptableObjects
{
    public class PMRDialogueSO : PMRGraphSO
    {
        [field: SerializeField] [field: TextArea()] public string Text { get; set; }
        
        [field: SerializeField] public PMRGraphSO NextNode { get; set; }
        [field: SerializeField] public bool IsStartingDialogue { get; set; }
        
    }
}
