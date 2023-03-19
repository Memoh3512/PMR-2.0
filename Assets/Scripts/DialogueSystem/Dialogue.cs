using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace PMR
{
    using ScriptableObjects;
    
    public class Dialogue : MonoBehaviour
    {

        //Dialogue scriptable objects
        [SerializeField] private PMRContainerSO dialogueContainer;
        [SerializeField] private PMRGroupSO dialogueGroup;
        [SerializeField] public PMRDialogueSO dialogue;
        
        //filters
        [SerializeField] private bool groupedDialogues;
        [SerializeField] private bool startingDialoguesOnly;
        
        //indexes
        [SerializeField] private string selectedGroup;
        [SerializeField] private string selectedDialogue;
        
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
