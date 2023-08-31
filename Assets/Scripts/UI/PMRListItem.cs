using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PMR
{
    public class PMRListItem : ScriptableObject
    {
        [field: SerializeField] public string itemName { get; private set; }
        [field: SerializeField, TextArea] public string itemDescription { get; private set; }
        [field: SerializeField] public Sprite itemSprite { get; private set; }
    }
}
