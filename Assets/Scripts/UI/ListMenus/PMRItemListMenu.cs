using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PMR
{
    using Item;

    public class PMRItemListMenu : PMRListMenu<PMRItem>
    {
        public List<PMRItem> testListItem;
        
        private void Start()
        {
            //Example of how to start list item menu
            SetTooltipText("Throw away which one?");
            InitializeListMenu(testListItem);
            OnItemSelected.AddListener((item) =>
            {
                CloseMenu();
                Debug.Log($"Use {item.itemName}!");
            });
            
            OpenMenu();
        }
    }
}
