using UnityEngine;

namespace PMR.ScriptableObjects
{
    public class PMRDIalogueSO : PMRGraphSO
    {
        [field: SerializeField] [field: TextArea()] public string Text { get; set; }
        [field: SerializeField] public bool IsStartingDialogue { get; set; }

        public void Initialize(string dialogueName, string text, bool isStartingDialogue)
        {
            Initialize(dialogueName);
            Text = text;
            IsStartingDialogue = isStartingDialogue;
        }
    }
}
