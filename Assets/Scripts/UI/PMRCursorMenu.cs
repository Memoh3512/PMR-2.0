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
                SpawnCursor();
            }
        }

        private void SpawnCursor()
        {
            GameObject cursorPrefab = PMRSettings.menuSettings.DefaultMenuCursor;

            GameObject cursorInstance = Instantiate(cursorPrefab, transform);

            cursorInstance.transform.position = defaultSelectedItem.transform.position;

        }
    }
}
