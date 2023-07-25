using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PMR
{
    public class PMRSelectable : MonoBehaviour
    {
        public Vector2 cursorOffset;
        public PMRSelectable leftElement;
        public PMRSelectable rightElement;
        public PMRSelectable upElement;
        public PMRSelectable downElement;

        //Events
        public UnityEvent OnCursorEnter;
        public UnityEvent OnCursorExit;
        public UnityEvent OnSelect;

        public void Select()
        {
            OnSelect.Invoke();
        }

        public void OnEnter()
        {
            OnCursorEnter.Invoke();
        }

        public void OnExit()
        {
            OnCursorExit.Invoke();
        }
    }
}
