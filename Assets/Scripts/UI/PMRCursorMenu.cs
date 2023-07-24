using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PMR
{
    public class PMRCursorMenu : MonoBehaviour
    {
        [SerializeField] private PMRSelectable defaultSelectedItem;

        [SerializeField] private bool spawnCursorOnStart = true;

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

            GameObject cursorInstance = Instantiate(cursorPrefab, transform);

            PMRMenuCursor cursorComponent = cursorInstance.GetComponent<PMRMenuCursor>();

            if (cursorComponent is null)
            {
                Debug.LogError($"Trying to instantiate menu cursor prefab that has no cursor component! {cursorPrefab.name}");
                return;
            }
            
            cursorComponent.Init(selectedItem);
        }
    }
}
