using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PMR
{
    public abstract class PMRListItem : ScriptableObject
    {
        [field: SerializeField] public string itemName { get; private set; }
        [field: SerializeField, TextArea] public string itemDescription { get; private set; }
        [field: SerializeField] public Sprite itemSprite { get; private set; }
        [field: SerializeField] public Sprite itemDisabledSprite { get; private set; }
        
        //Makes checks to see if the item is enabled - checks depend on the type of list item
        public abstract bool CanUse();
    }
}
