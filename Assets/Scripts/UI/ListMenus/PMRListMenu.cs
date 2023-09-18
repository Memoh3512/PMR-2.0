using System;
using System.Collections;
using System.Collections.Generic;
using Febucci.UI;
using PMR.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace PMR
{
    [RequireComponent(typeof(PMRCursorMenu))]
    public class PMRListMenu <ListItemType> : MonoBehaviour, IPMRMenu 
           where ListItemType : PMRListItem
    {
        
        [Header("Objects")]
        [SerializeField] private RectTransform listContainer;
        [SerializeField] private GameObject itemPrefab;
        [SerializeField] private Image titleBackground;
        [SerializeField] private TextMeshProUGUI DescriptionTextObject;
        [SerializeField] private TextMeshProUGUI TitleTextObject;
        [SerializeField] private TextMeshProUGUI TooltipTextObject;
        [SerializeField] private Image UpArrowObject;
        [SerializeField] private Image DownArrow;

        [Header("Visual")]
        [SerializeField] private Color titleColor;
        [SerializeField] private float verticalPadding;
        [SerializeField] private float itemSpacing;

        [Header("Scroll")] 
        [SerializeField] private ScriptedTimeCurveVector3 scrollTimeCurve;

        [Header("Content")]
        [SerializeField] private string titleText;
        private string tooltipText;
        
        [Header("Events")]
        public UnityEvent<ListItemType> OnItemSelected;
        public UnityEvent<ListItemType, int> OnItemHovered;
        
        private PMRCursorMenu cursorMenuComponent;

        public void SetTooltipText(string text)
        {
            tooltipText = text;
        }
        
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
            
            //setup title and tooltip
            if (TitleTextObject != null) TitleTextObject.text = titleText;
            titleBackground.color = titleColor;
            if (TooltipTextObject != null) TooltipTextObject.text = tooltipText;

            float containerHeight = listContainer.GetComponent<RectTransform>().sizeDelta.y;
            float prefabHeight = itemPrefab.GetComponent<RectTransform>().sizeDelta.y;
            
            float currentY = -(verticalPadding + prefabHeight/2.0f) + (containerHeight/2.0f);
            
            PMRSelectable lastItem = null;
            PMRSelectable firstItem = null;
            int index = 0;
            foreach (ListItemType item in items)
            {
                int itemIndex = index + 1;
                
                GameObject itemInstance = Instantiate(itemPrefab, listContainer.transform);

                itemInstance.name = $"ListItem - {item.name}";
                
                //Set Text
                itemInstance.GetComponent<PMRListItemUI>().SetItem(item);

                //Set Navigation
                PMRSelectable itemSelectableComp = itemInstance.GetComponent<PMRSelectable>();
                if (firstItem == null)
                {
                    firstItem = itemSelectableComp;
                    SetDescriptionText(item.itemDescription);
                }
                else if (lastItem != null)
                {
                    lastItem.downElement = itemSelectableComp;
                    itemSelectableComp.upElement = lastItem;
                }
                
                lastItem = itemSelectableComp;
                
                //Set Actions
                itemSelectableComp.OnSelect.AddListener(() => OnItemClicked(item));
                itemSelectableComp.OnCursorEnter.AddListener(() => ItemHovered(item, itemIndex));

                //Set Pos
                Vector3 localPosition = itemInstance.transform.localPosition;
                localPosition = new Vector3(localPosition.x, currentY, localPosition.z);
                itemInstance.transform.localPosition = localPosition;

                currentY -= (itemSpacing + prefabHeight);

                index = itemIndex;
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
            if (item.CanUse())
            {
                Debug.Log($"List Menu selected {item.itemName}!");
                OnItemSelected.Invoke(item);
            }
        }

        private void ItemHovered(ListItemType item, int index)
        {
            OnItemHovered.Invoke(item, index);
            SetDescriptionText(item.itemDescription);
        }

        void SetDescriptionText(string text)
        {
            if (DescriptionTextObject != null) DescriptionTextObject.text = text;
        }

        public virtual void OpenMenu()
        {
            //TODO Anim and select first item
        }

        public void CloseMenu()
        {
            //TODO Anim or maybe event

            OnItemSelected = null;
            Destroy(gameObject);
        }
    }
    
}