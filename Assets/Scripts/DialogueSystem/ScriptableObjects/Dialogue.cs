using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace PMR
{
    using ScriptableObjects;
    [CreateAssetMenu(fileName = "DialogueInstance", menuName = "PMR/Create Dialogue Instance", order = 0)]
    public class Dialogue : ScriptableObject
    {

        //Dialogue scriptable objects
        [SerializeField] private PMRContainerSO dialogueContainer;
        [SerializeField] private PMRGroupSO dialogueGroup;
        [SerializeField] public PMRGraphSO dialogue;
        
        //filters
        [SerializeField] private bool groupedDialogues;
        [SerializeField] private bool startingDialoguesOnly;
        
        //indexes
        [SerializeField] private string selectedGroup;
        [SerializeField] private string selectedDialogue;
    }
}
