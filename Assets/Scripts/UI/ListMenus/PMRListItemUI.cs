using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PMR
{
    public class PMRListItemUI : MonoBehaviour
    {
        public Image spriteRenderer;
        public TextMeshProUGUI textComponent;
        
        //private PMRListItem item;

        public void SetItem(PMRListItem item)
        {
            //this.item = item;
            spriteRenderer.sprite = item.itemSprite;
            textComponent.text = item.itemName;
        }
    }
}
