using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PMR
{
    public class PMRSelectable : MonoBehaviour
    {
        [SerializeField] private Vector2 cursorOffset;
        [SerializeField] private PMRSelectable leftElement;
        [SerializeField] private PMRSelectable rightElement;
        [SerializeField] private PMRSelectable upElement;
        [SerializeField] private PMRSelectable downElement;

        //Events
        public UnityEvent OnCursorEnter;
        public UnityEvent OnCursorExit;
        public UnityEvent OnSelect;

        public void Select()
        {
            OnSelect.Invoke();
        }
    }
}
