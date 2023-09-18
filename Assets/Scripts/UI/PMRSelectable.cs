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
        public UnityEvent<CursorSelectionChangeContext> OnTryCursorEnter; //Use to change the selection changed parameters depending on the case. Example: not animating when having to scroll menu


        public void Select()
        {
            OnSelect.Invoke();
        }

        public void OnEnter()
        {
            OnCursorEnter.Invoke();
        }

        public void OnTryEnter(CursorSelectionChangeContext context)
        {
            OnTryCursorEnter.Invoke(context);
        }

        public void OnExit()
        {
            OnCursorExit.Invoke();
        }
    }
}
