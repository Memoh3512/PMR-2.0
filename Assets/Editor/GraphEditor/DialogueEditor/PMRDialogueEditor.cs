using UnityEditor;

namespace PMR.GraphEditor
{
    public class PMRDialogueEditor : PMRGraphEditorWindow
    {
        [MenuItem("PMR/Dialogue Editor")]
        protected static void Open()
        {
            Open<PMRDialogueEditor>("Dialogue Editor");
        }

        public PMRDialogueEditor()
        {
            folderName = "DialogueEditor";
            defaultFilename = "MyDialogue"; //Make sure to set this value every time for arrangement
        }

        protected override void OnEnable()
        {
            AddGraphView(new PMRDialogueEditorGraphView(this));
            base.OnEnable();
        }
    }
}
