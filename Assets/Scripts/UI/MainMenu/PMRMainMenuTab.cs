using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PMR
{
    public class PMRMainMenuTab : MonoBehaviour
    {
        public Color backgroundColor1;
        public Color backgroundColor2;
        public GameObject tabContentRoot;

        public UnityEvent OnActivate;
        public UnityEvent OnDeactivate;
    }
}
