using PMR.GraphEditor.Elements;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace PMR.GraphEditor
{
    public class PMRCutsceneEditorSearchWindow : PMRSearchWindow
    {
        public PMRCutsceneEditorSearchWindow()
        {
            searchEntries.AddRange(new SearchTreeEntry[]
            {
                //add sections and elements here, line in DialogueEditorSearchWindow
            });
        }
    }
}