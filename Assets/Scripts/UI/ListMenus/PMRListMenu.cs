using System;
using System.Collections;
using System.Collections.Generic;
using Febucci.UI;
using PMR.ScriptableObjects;
using TMPro;
using UnityEngine;

namespace PMR
{
    [RequireComponent(typeof(PMRCursorMenu))]
    public class PMRListMenu <ListItemType> : MonoBehaviour, IPMRMenu 
           where ListItemType : PMRListItem
    {
        [SerializeField] private RectTransform listContainer;
        [SerializeField] private GameObject itemPrefab;
        [SerializeField] private TextMeshProUGUI DescriptionText;
        [SerializeField] private float verticalPadding;
        [SerializeField] private float itemSpacing;
        
        private PMRCursorMenu cursorMenuComponent;

        public Action<ListItemType> OnItemSelected;
        private void Awake()
        {
            cursorMenuComponent = GetComponent<PMRCursorMenu>();
        }

        public void InitializeListMenu(List<ListItemType> items)
        {
            if (itemPrefab is null)
            {
                Debug.LogError($"No valid item prefab to use! {name} {{PMRListMenu}}.InitializeListMenu");
                return;
            }

            float containerHeight = listContainer.GetComponent<RectTransform>().sizeDelta.y;
            float prefabHeight = itemPrefab.GetComponent<RectTransform>().sizeDelta.y;
            
            float currentY = -(verticalPadding + prefabHeight/2.0f) + (containerHeight/2.0f);
            
            PMRSelectable lastItem = null;
            PMRSelectable firstItem = null;
            foreach (ListItemType item in items)
            {
                GameObject itemInstance = Instantiate(itemPrefab, listContainer.transform);

                itemInstance.name = $"ListItem - {item.name}";
                
                //Set Text
                itemInstance.GetComponent<PMRListItemUI>().SetItem(item);

                //Set Navigation
                PMRSelectable itemSelectableComp = itemInstance.GetComponent<PMRSelectable>();
                if (firstItem == null) firstItem = itemSelectableComp;
                if (lastItem != null)
                {
                    lastItem.downElement = itemSelectableComp;
                    itemSelectableComp.upElement = lastItem;
                }
                
                lastItem = itemSelectableComp;
                
                //Set Action
                itemSelectableComp.OnSelect.AddListener(() => OnItemClicked(item));

                //Set Pos
                Vector3 localPosition = itemInstance.transform.localPosition;
                localPosition = new Vector3(localPosition.x, currentY, localPosition.z);
                itemInstance.transform.localPosition = localPosition;

                currentY -= (itemSpacing + prefabHeight);
            }
            
            //Loopover Navigation
            if (firstItem != null && lastItem != null && lastItem != firstItem)
            {
                firstItem.upElement = lastItem;
                lastItem.downElement = firstItem;   
            }
            
            //Spawn Cursor
            cursorMenuComponent.SpawnCursor(firstItem);
        }

        private void OnItemClicked(ListItemType item)
        {
            OnItemSelected(item);
            CloseMenu();
        }

        public void OpenMenu()
        {
            //TODO Anim
        }

        public void CloseMenu()
        {
            //TODO Anim or maybe event

            OnItemSelected = null;
            Destroy(gameObject);
        }
    }
    
}
