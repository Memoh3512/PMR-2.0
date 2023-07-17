using UnityEngine;
using Febucci.UI;
using PMR.ScriptableObjects;
using UnityEngine.Events;

   namespace PMR
{
    public class DialoguePlayer : MonoBehaviour, IGraphExecutor
    {

        [SerializeField] private bool playDialogueOnStart;
        [SerializeField] private Dialogue dialogueToAutoPlay;

        [SerializeField] private GameObject textPrefab;

        private GameObject textObject;
        private TypewriterByCharacter typeWriter;
        
        private PMRGraphSO nextNodeToExecute;
        private GraphExecutionContext currentContext;
        private bool DisplayingText = false;

        public UnityEvent OnDialogueEnd;
        public UnityEvent OnDialogueStart;
        public UnityEvent OnDialogueAdvance;

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

            textObject = Instantiate(textPrefab);
            
            if (textObject == null)
            {
                Debug.LogError($"Trying to play dialogue that has a null text object! {gameObject.name}");
            }

            typeWriter = textObject.GetComponentInChildren<TypewriterByCharacter>();
            typeWriter.onTextShowed.AddListener(OnTextShowed);
            
            currentContext = new GraphExecutionContext(this);
            //TODO context.Source = player;
            //TODO context.Target = other;
            ExecuteDialogueNode(dialogue.dialogue, currentContext);
            
            OnDialogueStart.Invoke();
        }

        void EndDialogue()
        {
            //clean up
            currentContext = null;
            nextNodeToExecute = null;
            
            typeWriter = null;
            Destroy(textObject);
            textObject = null;
            
            OnDialogueEnd.Invoke();
        }

        public void ExecuteDialogueNode(IGraphExecutable node, GraphExecutionContext context)
        {
            if (node == null)
            {
                //end of dialogue (we get here when previous node was text and there is no next node
                EndDialogue();
                return;
            }

            node.Execute(context, OnNodeExecutionFinished);
        }
        
        void OnNodeExecutionFinished(GraphExecutionResult result)
        {
            switch (result.Status)
            {
                case GraphExecutionStatus.Continue:
                    //nextNodeToExecute = result.NextNode; Re-add maybe? If needed
                    ExecuteDialogueNode(result.NextNode, currentContext);
                    break;
                case GraphExecutionStatus.Wait:
                    nextNodeToExecute = result.NextNode;
                    break;
                case GraphExecutionStatus.Stop:
                    EndDialogue();
                    break;
            }
        }

        public void TriggerText(string text)
        {
            if (typeWriter != null)
            {
                typeWriter.ShowText(text);
                typeWriter.StartShowingText(true);
                DisplayingText = true;
                
                OnDialogueAdvance.Invoke();
            }
        }
    }
}
