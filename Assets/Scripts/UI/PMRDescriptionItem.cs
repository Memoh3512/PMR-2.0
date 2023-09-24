using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PMR
{
    public class PMRDescriptionItem : MonoBehaviour
    {
        [SerializeField, TextArea] private string descriptionText;

        public string GetDescriptionText()
        {
            return descriptionText;
        }
    }
}
