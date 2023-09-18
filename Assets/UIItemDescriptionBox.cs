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
        
        //TODO Handle scrolling with buttons
    }
}
