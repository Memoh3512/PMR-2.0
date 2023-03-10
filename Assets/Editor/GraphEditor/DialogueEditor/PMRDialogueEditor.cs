using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PMR.GraphEditor
{
    public class PMRDialogueEditor : PMRGraphEditorWindow
    {
        [MenuItem("Window/PMR/Dialogue Editor")]
        protected static void Open()
        {
            Open<PMRDialogueEditor>("Dialogue Editor");
        }

        public PMRDialogueEditor()
        {
            folderName = "DialogueEditor";
            defaultFilename = "MyDialogueEditor"; //Make sure to set this value every time for arrangement
        }

        protected override void OnEnable()
        {
            AddGraphView(new PMRDialogueEditorGraphView(this));
            base.OnEnable();
        }
    }
}
