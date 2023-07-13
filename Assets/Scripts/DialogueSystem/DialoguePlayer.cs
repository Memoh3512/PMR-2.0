using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Febucci.UI;
using PMR.ScriptableObjects;

namespace PMR
{
    public class DialoguePlayer : MonoBehaviour
    {

        [SerializeField] private bool playDialogueOnStart;
        [SerializeField] private Dialogue dialogueToAutoPlay;

        [SerializeField] private GameObject textObject;
        
        // Start is called before the first frame update
        void Start()
        {
            if (playDialogueOnStart && dialogueToAutoPlay != null)
            {
                PlayDialogue(dialogueToAutoPlay);
            }
        }

        public void PlayDialogue(Dialogue dialogue)
        {
            if (textObject == null)
            {
                Debug.LogError($"Trying to play dialogue that has a null text object! {gameObject.name}");
            }

            DialogueExecutionContext context = new DialogueExecutionContext(this);
            //context.Source = player;
            //context.Target = other;
            ExecuteDialogueNode(dialogue.dialogue, context);
        }

        private void ExecuteDialogueNode(IDialogueExecutable node, DialogueExecutionContext context)
        {
            if (node == null)
            {
                Debug.LogError("Trying to execute null node in ExecuteDialogueNode!");
                return;
            }

            DialogueExecutionResult result = node.Execute(context);

            if (result.Status == DialogueExecutionStatus.Continue)
            {
                node.Execute(result.NextNode);
            }
        }

        public void TriggerText(string text)
        {
            TypewriterByCharacter typeWriter = textObject.GetComponent<TypewriterByCharacter>();
            if (typeWriter != null)
            {
                typeWriter.ShowText(text);
            }
        }
    }
}
