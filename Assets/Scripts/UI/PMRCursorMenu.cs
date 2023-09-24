using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PMR
{
    public class PMRCursorMenu : MonoBehaviour
    {
        [SerializeField] private PMRSelectable defaultSelectedItem;
        [SerializeField] private Transform cursorParent;

        [SerializeField] private bool spawnCursorOnStart = true;

        private PMRMenuCursor cursorInstance;

        private void Start()
        {
            if (spawnCursorOnStart)
            {
                SpawnCursor(defaultSelectedItem);
            }
        }

        public void SpawnCursor(PMRSelectable selectedItem)
        {
            GameObject cursorPrefab = PMRSettings.menuSettings.DefaultMenuCursor;

            GameObject newCursorInstance = Instantiate(cursorPrefab, cursorParent == null ? transform : cursorParent);

            cursorInstance = newCursorInstance.GetComponent<PMRMenuCursor>();

            if (cursorInstance is null)
            {
                Debug.LogError($"Trying to instantiate menu cursor prefab that has no cursor component! {cursorPrefab.name}");
                return;
            }
            
            cursorInstance.Init(selectedItem);
        }

        //use primarily to change selection scope for menus with multiple scopes. Leave newParent null to not change it
        public void SelectItem(PMRSelectable item)
        {
            if (item == null)
            {
                Debug.LogError("menu cursor trying to select a null item! Please verify your use of PMRCursorMenu.SelectItem.");
            }

            if (cursorInstance != null)
            {
                cursorInstance.ChangeSelection(item);
            }
        }

        public void SetCursorParent(Transform newParent)
        {
            if (cursorInstance != null && newParent != null)
            {
                cursorInstance.transform.SetParent(newParent.transform, false);
            }
        }
    }
}
