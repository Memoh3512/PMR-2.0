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
            InitializeListMenu(testListItem);
            OnItemSelected = (item) =>
            {
                Debug.Log($"Use {item.itemName}!");
            };
        }
    }
}
