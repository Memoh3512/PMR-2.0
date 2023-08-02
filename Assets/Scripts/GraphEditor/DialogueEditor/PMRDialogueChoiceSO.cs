using System;
using System.Collections.Generic;
using UnityEngine;

namespace PMR.ScriptableObjects
{
    public class PMRDialogueChoiceSO : PMRGraphSO
    {

        [field: SerializeField] [field: TextArea()] public string Text { get; set; }
        [field: SerializeField] public List<PMRDialogueChoiceSOData> Choices { get; set; }
        
        //Instance variables (to clean after finishing execution)
        private GraphExecutionContext currentContext;
        
        public override void Execute(GraphExecutionContext context)
        {
            currentContext = context;
            
            currentContext.DialoguePlayer.TriggerText(Text, true);
            
            currentContext.DialoguePlayer.OnTextEnd.AddListener(OnTextEnd);
        }

        void OnTextEnd()
        {
            DialoguePlayer dialoguePlayer = currentContext.DialoguePlayer;
            
            GameObject menuPrefab = dialoguePlayer.ChoiceMenu == null
                ? PMRSettings.menuSettings.DefaultChoiceMenu
                : dialoguePlayer.ChoiceMenu;
            GameObject choiceMenuInstance = Instantiate(menuPrefab);

            PMRChoiceMenu choiceMenuComponent = choiceMenuInstance.GetComponent<PMRChoiceMenu>();
            choiceMenuComponent.SetChoices(Choices);

            choiceMenuComponent.OnChoiceTaken = (choice) =>
            {
                currentContext.DialoguePlayer.OnTextEnd.RemoveListener(OnTextEnd);
                
                GraphExecutionStatus status = GraphExecutionStatus.Continue;
                currentContext.Finish((new GraphExecutionResult(status, choice.NextDialogue)));

                CleanupRuntimeData();
            };
        }

        void CleanupRuntimeData()
        {
            currentContext = null;
        }
    }
}
