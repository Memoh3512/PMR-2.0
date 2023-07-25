using System;
using System.Collections.Generic;
using UnityEngine;

namespace PMR.ScriptableObjects
{
    public class PMRDialogueChoiceSO : PMRGraphSO
    {

        [field: SerializeField] [field: TextArea()] public string Text { get; set; }
        [field: SerializeField] public List<PMRDialogueChoiceSOData> Choices { get; set; }
        
        public override void Execute(GraphExecutionContext context, Action<GraphExecutionResult> finishedCallback)
        {
            context.DialoguePlayer.TriggerText(Text, true);
            
            GameObject menuPrefab = context.DialoguePlayer.ChoiceMenu == null
                ? PMRSettings.menuSettings.DefaultChoiceMenu
                : context.DialoguePlayer.ChoiceMenu;
            GameObject choiceMenuInstance = Instantiate(menuPrefab);

            PMRChoiceMenu choiceMenuComponent = choiceMenuInstance.GetComponent<PMRChoiceMenu>();
            choiceMenuComponent.SetChoices(Choices);

            choiceMenuComponent.OnChoiceTaken += (choice) =>
            {
                GraphExecutionStatus status = GraphExecutionStatus.Continue;
                finishedCallback(new GraphExecutionResult(status, choice.NextDialogue));  
            };
        }
        
    }
}
