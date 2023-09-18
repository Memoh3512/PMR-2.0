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
        [SerializeField] private TextMeshProUGUI TitleTextObject;
        [SerializeField] private TextMeshProUGUI TooltipTextObject;
        [SerializeField] private Image upArrowObject;
        [SerializeField] private Image downArrowObject;

        [Header("Visual")]
        [SerializeField] private Color titleColor;
        [SerializeField] private float verticalPadding;
        [SerializeField] private float itemSpacing;

        [Header("Scroll")]
        [Min(0), Tooltip("Max number of items shown in list at the same time. Used for scrolling")]
        [SerializeField] private int maxItems;
        [SerializeField] private ScriptedTimeCurveVector2 scrollTimeCurve;
        
        private int scrollIndex;

        [Header("Content")]
        [SerializeField] private string titleText;
        private string tooltipText;
        
        private float containerHeight;
        private float prefabHeight;

        protected int itemsCount;
        
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

        private void Update()
        {
            //curve update
            if (scrollTimeCurve != null && scrollTimeCurve.IsStarted())
            {
                listContainer.transform.localPosition = scrollTimeCurve.Value();
            }
        }
        
        protected void InitializeListMenu(List<ListItemType> items)
        {
            if (itemPrefab is null)
            {
                Debug.LogError($"No valid item prefab to use! {name} {{PMRListMenu}}.InitializeListMenu");
                return;
            }

            itemsCount = items.Count;
            
            //setup title and tooltip
            if (TitleTextObject != null) TitleTextObject.text = titleText;
            titleBackground.color = titleColor;
            if (TooltipTextObject != null) TooltipTextObject.text = tooltipText;

            containerHeight = listContainer.GetComponent<RectTransform>().sizeDelta.y;
            prefabHeight = itemPrefab.GetComponent<RectTransform>().sizeDelta.y;
            
            float currentY = -(verticalPadding + prefabHeight/2.0f) + (containerHeight/2.0f);
            
            PMRSelectable lastItem = null;
            PMRSelectable firstItem = null;
            int index = 0;
            foreach (ListItemType item in items)
            {
                int itemIndex = index;
                
                GameObject itemInstance = Instantiate(itemPrefab, listContainer.transform);

                itemInstance.name = $"ListItem - {item.name}";

                //Set Pos
                Vector3 localPosition = itemInstance.transform.localPosition;
                localPosition = new Vector3(localPosition.x, currentY, localPosition.z);
                itemInstance.transform.localPosition = localPosition;

                currentY -= (itemSpacing + prefabHeight);
                
                //Set Text
                itemInstance.GetComponent<PMRListItemUI>().SetItem(item);
                
                //Set Navigation
                PMRSelectable itemSelectableComp = itemInstance.GetComponent<PMRSelectable>();
                if (firstItem == null)
                {
                    firstItem = itemSelectableComp;
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
                itemSelectableComp.OnTryCursorEnter.AddListener((context) => ProcessSelectionChangedContext(context, itemIndex));

                index = itemIndex + 1;
            }
            
            //Loopover Navigation
            if (firstItem != null && lastItem != null && lastItem != firstItem)
            {
                firstItem.upElement = lastItem;
                lastItem.downElement = firstItem;   
            }
            
            UpdateArrowVisibility();
            
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

            int calculatedScrollIndex = CalculateScrollIndex(index);
            //Debug.Log($"INDEX IS {index} SCROLLINDEX IS {calculatedScrollIndex}");

            if (calculatedScrollIndex != scrollIndex)
            {
                scrollIndex = calculatedScrollIndex;
                
                UpdateArrowVisibility();
                
                Vector2 scrollPosition = new Vector2(0, scrollIndex * (itemSpacing + prefabHeight));
                
                if (scrollTimeCurve == null)
                {
                    listContainer.transform.localPosition = scrollPosition;
                }
                else
                {
                    scrollTimeCurve.SetValues(listContainer.transform.localPosition, scrollPosition);
                    scrollTimeCurve.Start();
                }
            }
        }

        void ProcessSelectionChangedContext(CursorSelectionChangeContext context, int index)
        {
            //move to container desired position
            context.OverridePosition = true;
            int clampedIndex = Math.Clamp(index - scrollIndex, 0, maxItems - 1);
            context.OverriddenPosition = GetItemPosition(clampedIndex);
        }
        
        private Vector2 GetItemPosition(int index)
        {
            Transform containerTransform = listContainer.transform;
            return containerTransform.GetChild(index).localPosition;
        }
        
        private int CalculateScrollIndex(int index)
        {
            if (index >= scrollIndex + maxItems)
            {
                return index - maxItems + 1;
            }

            if (index < scrollIndex)
            {
                return index;
            }

            return scrollIndex;
        }

        private void UpdateArrowVisibility()
        {
            if (upArrowObject != null)
            {
                upArrowObject.enabled = scrollIndex > 0;
            }

            if (downArrowObject != null)
            {
                downArrowObject.enabled = scrollIndex < (itemsCount - maxItems);
            }
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
