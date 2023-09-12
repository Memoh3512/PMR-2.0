using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace PMR
{
    public class PMRMenuCursor : MonoBehaviour
    {

        private PMRSelectable selectedItem;

        [FormerlySerializedAs("OnSelectItem")] [SerializeField] private UnityEvent onSelectItem;
        [FormerlySerializedAs("OnSelectionChanged")] [SerializeField] private UnityEvent onSelectionChanged;
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
            if (positionCurve.IsStarted())
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
            //TODO Play menu select sound
        }

        void ChangeSelection(PMRSelectable newItem, bool sendChangedEvent = true)
        {
            if (newItem == selectedItem || newItem is null) return;

            if (selectedItem != null) selectedItem.OnExit();
            selectedItem = newItem;
            selectedItem.OnEnter();
            
            //movement
            Vector2 newPosition = newItem.transform.position + (Vector3)newItem.cursorOffset;
            
            if (positionCurve == null)
            {
                transform.position = newPosition;
            }
            else
            {
                positionCurve.SetValues(transform.position, newPosition);
                positionCurve.Start();
            }
            
            if (sendChangedEvent) onSelectionChanged.Invoke();
        }
    }
}
