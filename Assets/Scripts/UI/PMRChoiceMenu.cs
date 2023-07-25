using System;
using System.Collections;
using System.Collections.Generic;
using Febucci.UI;
using PMR.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace PMR
{
    [RequireComponent(typeof(PMRCursorMenu))]
    public class PMRChoiceMenu : MonoBehaviour, IPMRMenu
    {

        [SerializeField] private RectTransform container;
        [SerializeField] private GameObject choicePrefab;
        [SerializeField] private float verticalPadding;
        [SerializeField] private float itemSpacing;
        
        private PMRCursorMenu cursorMenuComponent;

        public Action<PMRDialogueChoiceSOData> OnChoiceTaken;
        private void Awake()
        {
            cursorMenuComponent = GetComponent<PMRCursorMenu>();
        }

        public void SetChoices(List<PMRDialogueChoiceSOData> choices)
        {
            GameObject choicePrefabToUse = choicePrefab != null ? choicePrefab : PMRSettings.menuSettings.DefaultChoicePrefab;

            if (choicePrefabToUse is null)
            {
                Debug.LogError("No valid choice prefab to use! PMRChoiceMenu.SetChoices");
                return;
            }

            Vector2 prefabSize = choicePrefabToUse.GetComponent<RectTransform>().sizeDelta;
            float prefabHeight = prefabSize.y;
            
            float heightNeeded = (choices.Count * prefabHeight) + 
                               (2 * verticalPadding) + 
                               ((choices.Count-1) * itemSpacing);

            //set container size
            container.GetComponent<RectTransform>().sizeDelta = new Vector2(prefabSize.x, heightNeeded);
            
            float currentY = -(verticalPadding + prefabHeight/2.0f);
            PMRSelectable lastChoice = null;
            PMRSelectable firstChoice = null;
            foreach (PMRDialogueChoiceSOData choice in choices)
            {
                GameObject choiceItem = Instantiate(choicePrefabToUse, container.transform);

                choiceItem.name = $"Choice ({choice.Text})";
                
                //Set Text
                choiceItem.GetComponent<TextAnimator_TMP>().SetText(choice.Text);

                //Set Navigation
                PMRSelectable itemSelectableComp = choiceItem.GetComponent<PMRSelectable>();
                if (firstChoice == null) firstChoice = itemSelectableComp;
                if (lastChoice != null)
                {
                    lastChoice.downElement = itemSelectableComp;
                    itemSelectableComp.upElement = lastChoice;
                }
                
                lastChoice = itemSelectableComp;
                
                //Set Action
                itemSelectableComp.OnSelect.AddListener(() => OnChoiceClick(choice));

                //Set Pos
                Vector3 localPosition = choiceItem.transform.localPosition;
                localPosition = new Vector3(localPosition.x, currentY, localPosition.z);
                choiceItem.transform.localPosition = localPosition;

                currentY -= (itemSpacing + prefabHeight);
            }
            
            //Loopover Navigation
            if (firstChoice != null && lastChoice != null && lastChoice != firstChoice)
            {
                firstChoice.upElement = lastChoice;
                lastChoice.downElement = firstChoice;   
            }
            
            //Spawn Cursor
            cursorMenuComponent.SpawnCursor(firstChoice);
        }

        private void OnChoiceClick(PMRDialogueChoiceSOData choice)
        {
            OnChoiceTaken(choice);
            CloseMenu();
        }

        public void OpenMenu()
        {
            
        }

        public void CloseMenu()
        {
            //TODO Anim or maybe event
            Destroy(gameObject);
        }
    }
}
