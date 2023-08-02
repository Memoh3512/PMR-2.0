using System.ComponentModel;
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
        
        [Category("Customization")] 
        public GameObject ChoiceMenu;
        [Category("Customization")] 
        public GameObject ChoicePrefab;

        private GameObject textObject;
        private TypewriterByCharacter typeWriter;
        
        private PMRGraphSO nextNodeToExecute;
        private GraphExecutionContext currentContext;
        private bool DisplayingText = false;
        private bool IsChoice = false;

        public UnityEvent OnDialogueStart;
        public UnityEvent OnTextEnd;
        public UnityEvent OnDialogueAdvance;
        public UnityEvent OnDialogueEnd;

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
                else if (!IsChoice)
                {
                    ExecuteDialogueNode(nextNodeToExecute, currentContext);
                }
            }
        }

        void OnTextShowed()
        {
            DisplayingText = false;
            OnTextEnd.Invoke();
        }

        /// <summary>
        /// Starts the dialogue. Will only be called once on a given text object, since the object will be destroyed at the end of the dialogue
        /// </summary>
        /// <param name="dialogue"></param>
        public void PlayDialogue(Dialogue dialogue)
        {
            textObject = Instantiate(textPrefab);
            
            if (textObject == null)
            {
                Debug.LogError($"Trying to play dialogue that has a null text object! {gameObject.name}");
            }

            typeWriter = textObject.GetComponentInChildren<TypewriterByCharacter>();
            typeWriter.onTextShowed.AddListener(OnTextShowed);
            
            currentContext = new GraphExecutionContext(this);
            currentContext.FinishedCallback = OnNodeExecutionFinished;
            
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

            if (typeWriter != null)
            {
                typeWriter.onTextShowed.RemoveAllListeners();
                typeWriter = null;   
            }
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

            node.Execute(context);
        }
        
        void OnNodeExecutionFinished(GraphExecutionResult result)
        {
            switch (result.Status)
            {
                case GraphExecutionStatus.Continue:
                    //nextNodeToExecute = result.NextNode; //Re-add maybe? If needed
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

        public void TriggerText(string text, bool isChoice = false)
        {
            if (typeWriter != null)
            {
                typeWriter.ShowText(text);
                typeWriter.StartShowingText(true);
                DisplayingText = true;
                IsChoice = isChoice;
                
                OnDialogueAdvance.Invoke();
            }
        }
    }
}
