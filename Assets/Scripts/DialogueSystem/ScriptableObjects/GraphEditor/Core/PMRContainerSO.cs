using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PMR.ScriptableObjects
{
    public class PMRContainerSO : ScriptableObject
    {
        [field: SerializeField]public string FileName { get; set; }
        [field: SerializeField]public SerializableDictionary<PMRGroupSO, List<PMRGraphSO>> Groups { get; set; }
        [field: SerializeField]public List<PMRGraphSO> UngroupedNodes { get; set; }

        public void Initialize(string fileName)
        {
            FileName = fileName;
            Groups = new SerializableDictionary<PMRGroupSO, List<PMRGraphSO>>();
            UngroupedNodes = new List<PMRGraphSO>();
        }

        public List<PMRGroupSO> GetGroups()
        {
            List<PMRGroupSO> groups = new List<PMRGroupSO>();
            groups.AddRange(Groups.Keys);
            return groups;
        }
        public List<string> GetGroupNames()
        {
            List<string> groupNames = new List<string>();
            foreach (PMRGroupSO group in Groups.Keys)
            {
                groupNames.Add(group.Name);
            }
            return groupNames;
        }

        public List<string> GetGroupedDialogueNames(PMRGroupSO group)
        {
            List<PMRGraphSO> groupedDialogues = Groups[group];

            List<string> dialogueNames = new List<string>();

            foreach (PMRGraphSO dialogue in groupedDialogues)
            {
                if (dialogue is PMRDialogueSO or PMRDialogueChoiceSO)
                {
                    dialogueNames.Add(dialogue.Name);
                }
            }

            return dialogueNames;
        }

        public List<string> GetUngroupedDialogueNames()
        {
            List<string> dialogueNames = new List<string>();
            foreach (PMRGraphSO dialogue in UngroupedNodes)
            {
                if (dialogue is PMRDialogueSO or PMRDialogueChoiceSO)
                {
                    dialogueNames.Add(dialogue.Name);
                }
            }

            return dialogueNames;
        }

    }
}
