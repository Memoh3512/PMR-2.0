using System.Collections;
using System.Collections.Generic;
using PMR.ScriptableObjects;
using UnityEngine;

namespace PMR.ScriptableObjects
{
    public class PMRDialogueChoiceSO : PMRGraphSO
    {

        [field: SerializeField] [field: TextArea()] public string Text { get; set; }
        [field: SerializeField] public List<PMRDialogueChoiceSOData> Choices { get; set; }
        
    }
}
