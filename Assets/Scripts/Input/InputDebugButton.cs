using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PMR
{
    public class InputDebugButton : MonoBehaviour
    {
        [SerializeField] private Sprite unpressed, pressed;
        
        public void SetPressed(bool isPressed)
        {
            GetComponent<Image>().sprite = isPressed ? pressed : unpressed;
        }
    }
}
