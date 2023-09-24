using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace PMR
{
    public class UIItemDescriptionBox : MonoBehaviour
    {
        public TextMeshProUGUI text;
        public void SetDescriptionItem(PMRListItem item, int itemIndex)
        {
            if (text != null)
            {
                text.text = item.itemDescription;
            }
        }
        
        public void SetDescriptionText(string newText)
        {
            if (text != null)
            {
                text.text = newText;
            }
        }
        
        public void SetDescriptionText(PMRDescriptionItem descriptionItem)
        {
            if (text != null)
            {
                text.text = descriptionItem.GetDescriptionText();
            }
        }
        
        //TODO Handle scrolling with buttons
    }
}
