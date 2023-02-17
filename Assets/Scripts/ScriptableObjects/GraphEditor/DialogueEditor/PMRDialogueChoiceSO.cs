using System.Collections;
using System.Collections.Generic;
using PMR.Data;
using PMR.ScriptableObjects;
using UnityEngine;

namespace PMR
{
    public class PMRDialogueChoiceSO : PMRDIalogueSO
    {

        [field: SerializeField] public List<PMRDialogueChoiceData> Choices;
        public void Initialize(string dialogueName, string text, bool isStartingDialogue, List<PMRDialogueChoiceData> choices)
        {
            Initialize(dialogueName, text, isStartingDialogue);
            Choices = choices;

        }
    }
}
