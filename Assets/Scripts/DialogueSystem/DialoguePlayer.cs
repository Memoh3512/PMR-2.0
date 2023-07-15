   using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Febucci.UI;
using PMR.ScriptableObjects;

namespace PMR
{
    public class DialoguePlayer : MonoBehaviour, IGraphExecutor
    {

        [SerializeField] private bool playDialogueOnStart;
        [SerializeField] private Dialogue dialogueToAutoPlay;

        [SerializeField] private GameObject textObject;

        private TypewriterByCharacter typeWriter;
        private PMRGraphSO nextNodeToExecute;
        private GraphExecutionContext currentContext;

        private bool DisplayingText = false;
        // Start is called before the first frame update
        void Start()
        {
            if (playDialogueOnStart && dialogueToAutoPlay != null)
            {
                PlayDialogue(dialogueToAutoPlay);
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (DisplayingText)
                {
                    typeWriter.SkipTypewriter();
                }
                else
                {
                    ExecuteDialogueNode(nextNodeToExecute, currentContext);
                }
            }
        }

        void OnTextShowed()
        {
            DisplayingText = false;
        }

        /// <summary>
        /// Starts the dialogue. Will only be called once on a given text object, since the object will be destroyed at the end of the dialogue
        /// </summary>
        /// <param name="dialogue"></param>
        public void PlayDialogue(Dialogue dialogue)
        {
            
            //TODO spawn dialogue box and hookup to it
            if (textObject == null)
            {
                Debug.LogError($"Trying to play dialogue that has a null text object! {gameObject.name}");
            }

            typeWriter = textObject.GetComponent<TypewriterByCharacter>();
            typeWriter.onTextShowed.AddListener(OnTextShowed);
                
            currentContext = new GraphExecutionContext(this);
            //TODO context.Source = player;
            //TODO context.Target = other;
            ExecuteDialogueNode(dialogue.dialogue, currentContext);
        }

        public void ExecuteDialogueNode(IGraphExecutable node, GraphExecutionContext context)
        {
            if (node == null)
            {
                Debug.LogError("Trying to execute null node in ExecuteDialogueNode!");
                return;
            }

            GraphExecutionResult result = node.Execute(context);

            if (result.Status == GraphExecutionStatus.Continue)
            {
                result.NextNode.Execute(context);
                nextNodeToExecute = result.NextNode;
            } else if (result.Status == GraphExecutionStatus.Wait)
            {
                nextNodeToExecute = result.NextNode;
            }
        }

        public void TriggerText(string text)
        {
            if (typeWriter != null)
            {
                typeWriter.ShowText(text);
                DisplayingText = true;
            }
        }
    }
}
