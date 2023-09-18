using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace PMR
{
    public class CursorSelectionChangeContext
    {
        private bool moveCursor = true;
        private bool animate = true;
        private bool callSelectionChanged = true;
        private bool overridePosition = false;
        private Vector3 overriddenPosition = Vector3.zero;

        public bool MoveCursor
        {
            get => moveCursor;
            set => moveCursor = value;
        }
        public bool Animate
        {
            get => animate;
            set => animate = value;
        }
        public bool CallSelectionChanged
        {
            get => callSelectionChanged;
            set => callSelectionChanged = value;
        }
        public bool OverridePosition
        {
            get => overridePosition;
            set => overridePosition = value;
        }
        public Vector2 OverriddenPosition
        {
            get => overriddenPosition;
            set => overriddenPosition = value;
        }
    }
    
    public class PMRMenuCursor : MonoBehaviour
    {

        private PMRSelectable selectedItem;

        [FormerlySerializedAs("OnSelectItem")] [SerializeField] private UnityEvent onSelectItem;
        [FormerlySerializedAs("OnSelectionChanged")] [SerializeField] private UnityEvent onSelectionChanged;
        [SerializeField] private UnityEvent<CursorSelectionChangeContext> OnTrySelectionChanged; //Use to change the selection changed parameters depending on the case. Example: not animating when having to scroll menu
        [SerializeField] private ScriptedTimeCurveVector2 positionCurve;

        public void Init(PMRSelectable initialSelectedItem)
        {
            ChangeSelection(initialSelectedItem, false);
        }

        // Update is called once per frame
        void Update()
        {
            if (selectedItem is null) return;
            
            //anim
            if (positionCurve.IsStartedNotElapsed())
            {
                transform.position = positionCurve.Value();
            } 
                
            //Temporary Input method
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                ChangeSelection(selectedItem.leftElement);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                ChangeSelection(selectedItem.rightElement);
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                ChangeSelection(selectedItem.upElement);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                ChangeSelection(selectedItem.downElement);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SelectItem(selectedItem);
            }
        }

        void SelectItem(PMRSelectable item)
        {
            if (item is null) return;
            
            item.Select();
        }

        void Move(PMRSelectable newItem, CursorSelectionChangeContext context)
        {
            Vector2 newPosition;
            if (context.OverridePosition)
            {
                newPosition = context.OverriddenPosition;
            }
            else
            {
                newPosition = newItem.transform.position + (Vector3)newItem.cursorOffset;
            }
            
            if (context.Animate == false || positionCurve == null || selectedItem == null)
            {
                transform.position = newPosition;
            }
            else
            {
                positionCurve.SetValues(transform.position, newPosition);
                positionCurve.Start();
            }
        }

        void ChangeSelection(PMRSelectable newItem, bool sendChangedEvent = true)
        {
            if (newItem == selectedItem || newItem is null) return;

            CursorSelectionChangeContext context = new CursorSelectionChangeContext();
            OnTrySelectionChanged.Invoke(context);

            if (context.MoveCursor)
            {
                Move(newItem, context);
            }

            //selection
            if (selectedItem != null) selectedItem.OnExit();
            selectedItem = newItem;
            selectedItem.OnEnter();
            
            if (sendChangedEvent && context.CallSelectionChanged) onSelectionChanged.Invoke();
        }
    }
}
