using System;
using Febucci.UI;
using UnityEngine;

namespace PMR
{
    [RequireComponent(typeof(TextAnimator_TMP))]
    public class TextAnimationsStartDisabled : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<TextAnimator_TMP>().SetBehaviorsActive(false);
        }
    }
}
