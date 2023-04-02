using PMR.GraphEditor.Elements;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace PMR.GraphEditor
{
    public class PMRDialogueEditorSearchWindow : PMRSearchWindow
    {
        public PMRDialogueEditorSearchWindow()
        {
            searchEntries.AddRange(new SearchTreeEntry[]
            {
                new SearchTreeGroupEntry(new GUIContent("Dialogue Node"), 1),
                new SearchTreeEntry(new GUIContent("Text", indentationIcon))
                {
                    level = 2,
                    userData = typeof(DialogueEditorTextNode)
                },
                new SearchTreeEntry(new GUIContent("Choice", indentationIcon))
                {
                    level = 2,
                    userData = typeof(DialogueEditorChoiceNode)
                },
            });
        }
    }
}