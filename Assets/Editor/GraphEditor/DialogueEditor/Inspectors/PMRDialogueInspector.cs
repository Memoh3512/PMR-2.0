using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PMR.Inspectors
{
    using ScriptableObjects;
    using GraphEditor.Utilities;
    
    [CustomEditor(typeof(Dialogue))]
    public class PMRDialogueInspector : Editor
    {

        //scriptable objects
        private SerializedProperty containerProperty;
        private SerializedProperty groupProperty;
        private SerializedProperty dialogueProperty;
        
        //filters
        private SerializedProperty groupedDialoguesProperty;
        private SerializedProperty startingDialoguesOnlyProperty;
        
        //Indexes
        private SerializedProperty selectedGroupProperty;
        private SerializedProperty selectedDialogueProperty;

        private void OnEnable()
        {
            containerProperty = serializedObject.FindProperty("dialogueContainer");
            groupProperty = serializedObject.FindProperty("dialogueGroup");
            dialogueProperty = serializedObject.FindProperty("dialogue");
            
            groupedDialoguesProperty = serializedObject.FindProperty("groupedDialogues");
            startingDialoguesOnlyProperty = serializedObject.FindProperty("startingDialoguesOnly");
            
            selectedGroupProperty = serializedObject.FindProperty("selectedGroup");
            selectedDialogueProperty = serializedObject.FindProperty("selectedDialogue");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            DrawDialogueContainerArea();
            
            PMRContainerSO container = (PMRContainerSO) containerProperty.objectReferenceValue;

            if (container == null)
            {
                StopDrawing("Select a Dialogue Container to see the rest of the Inspector");
                return;
            }
            
            PMRInspectorUtility.DrawSpace();
            DrawFiltersArea();

            bool startingDialoguesOnly = startingDialoguesOnlyProperty.boolValue;

            List<string> dialogueNames;
            List<PMRGraphSO> dialogues;

            string dialogueInfoMessage;
            
            if (groupedDialoguesProperty.boolValue)
            {
                List<string> groupNames = container.GetGroupNames();
                if (groupNames.Count == 0)
                {
                    StopDrawing("There are no dialogue groups in this container!");
                    return;
                }
                PMRInspectorUtility.DrawSpace();
                DrawDialogueGroupsArea(container, container.GetGroups(), groupNames);

                PMRGroupSO group = (PMRGroupSO)groupProperty.objectReferenceValue;
                dialogueNames = container.GetGroupedNodeNames(group, startingDialoguesOnly, 
          typeof(PMRDialogueSO), 
                    typeof(PMRDialogueChoiceSO));
                dialogues = container.GetGroupedNodes(
                    group,
                    startingDialoguesOnly,
          typeof(PMRDialogueSO), 
                    typeof(PMRDialogueChoiceSO)
                    );
                dialogueInfoMessage = "There are no" + (startingDialoguesOnly ? " Starting" : "") + " Dialogues in this Dialogue Group!";
            }
            else
            {
                dialogueNames = container.GetUngroupedNodeNames(startingDialoguesOnly, 
          typeof(PMRDialogueSO), 
                    typeof(PMRDialogueChoiceSO));
                dialogues = container.GetUngroupedNodes(
                    startingDialoguesOnly,
          typeof(PMRDialogueSO),
                    typeof(PMRDialogueChoiceSO));
                dialogueInfoMessage = "There are no" + (startingDialoguesOnly ? " Starting" : "") + " Ungrouped Dialogues in this Graph!";
            }

            if (dialogueNames.Count == 0)
            {
                StopDrawing(dialogueInfoMessage);
                return;
            }
            
            PMRInspectorUtility.DrawSpace();
            DrawDialogueArea(dialogues, dialogueNames);

            serializedObject.ApplyModifiedProperties();
        }

        private void StopDrawing(string reason, MessageType messageType = MessageType.Info)
        {
            PMRInspectorUtility.DrawHelpBox(reason, messageType);
            
            PMRInspectorUtility.DrawSpace();
            
            PMRInspectorUtility.DrawHelpBox("You need to select a dialogue for this component to work!", MessageType.Warning);
            
            serializedObject.ApplyModifiedProperties();
        }

        #region Draw Methods
        private void DrawDialogueContainerArea()
        {
            PMRInspectorUtility.DrawHeader("Dialogue Container");

            containerProperty.DrawPropertyField();
        }
        private void DrawFiltersArea()
        {
            PMRInspectorUtility.DrawHeader("Filters");

            groupedDialoguesProperty.DrawPropertyField();
            startingDialoguesOnlyProperty.DrawPropertyField();
        }
        private void DrawDialogueGroupsArea(PMRContainerSO container, List<PMRGroupSO> groups, List<string> groupNames)
        {
            PMRInspectorUtility.DrawHeader("Dialogue Group");

            int selectedGroupIndex = PMRInspectorUtility.DrawPopup("Dialogue Group", selectedGroupProperty, groupNames);

            PMRGroupSO selectedGroup = groups[selectedGroupIndex];
            groupProperty.objectReferenceValue = selectedGroup;
            
            PMRInspectorUtility.DrawDisabledFields(() => groupProperty.DrawPropertyField());
        }
        private void DrawDialogueArea(List<PMRGraphSO> dialogues, List<string> dialogueNames)
        {
            PMRInspectorUtility.DrawHeader("Dialogue");

            int selectedDialogueIndex = PMRInspectorUtility.DrawPopup("Dialogue", selectedDialogueProperty, dialogueNames);

            PMRGraphSO selectedDialogue = dialogues[selectedDialogueIndex];
            dialogueProperty.objectReferenceValue = selectedDialogue;
            
            PMRInspectorUtility.DrawDisabledFields(() => dialogueProperty.DrawPropertyField());
        }
        #endregion
    }
}
