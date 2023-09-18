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
        private Vector3 firstListPos;
        private Vector3 lastListPos;

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
                    firstListPos = itemInstance.transform.position + (Vector3)itemSelectableComp.cursorOffset;
                }
                else if (lastItem != null)
                {
                    lastItem.downElement = itemSelectableComp;
                    itemSelectableComp.upElement = lastItem;
                }
                
                if (itemIndex + 1 == maxItems) //if this is the last item visible on list view, set last list pos
                {
                    lastListPos = itemInstance.transform.position + (Vector3)itemSelectableComp.cursorOffset;
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
            
            Vector2 scrollPosition;
            
            bool updateScroll = false;
            if (index >= scrollIndex + maxItems)
            {
                scrollIndex = index - maxItems + 1;
                updateScroll = true;
            } else if (index < scrollIndex)
            {
                scrollIndex = index;
                updateScroll = true;
            }
            
            //Debug.Log($"INDEX IS {index} SCROLLINDEX IS {scrollIndex}");

            if (updateScroll)
            {
                UpdateArrowVisibility();
                
                scrollPosition = new Vector2(0, scrollIndex * (itemSpacing + prefabHeight));
                
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

        void ProcessSelectionChangedContext(CursorSelectionChangeContext context, int index)
        {
            //don't allow moving if scrolling anim is still ongoing
            if (scrollTimeCurve.IsStartedNotElapsed())
            {
                context.AllowSelection = false;
                return;
            }
            
            //move cursor to first/last element position if overflowing
            if (index == 0 && scrollIndex > 0)
            {
                //overflow from bottom to top
                context.OverridePosition = true;
                context.OverriddenPosition = firstListPos;
                return;
            }
            if (index == itemsCount - 1 && scrollIndex == 0)
            {
                //overflow from top to bottom
                context.OverridePosition = true;
                context.OverriddenPosition = lastListPos;
                return;
            }
            
            //do not move cursor if we scroll from the interior
            if (index >= scrollIndex + maxItems || index < scrollIndex)
            {
                context.MoveCursor = false;
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
