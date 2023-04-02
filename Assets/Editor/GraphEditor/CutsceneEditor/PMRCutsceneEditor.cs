using UnityEditor;

namespace PMR.GraphEditor
{
    public class PMRCutsceneEditor : PMRGraphEditorWindow
    {
        [MenuItem("PMR/Cutscene Editor")]
        protected static void Open()
        {
            Open<PMRCutsceneEditor>("Cutscene Editor");
        }

        public PMRCutsceneEditor()
        {
            folderName = "CutsceneEditor";
            defaultFilename = "MyCutscene"; //Make sure to set this value every time for arrangement
        }

        protected override void OnEnable()
        {
            AddGraphView(new PMRCutsceneEditorGraphView(this));
            base.OnEnable();
        }
    }
}