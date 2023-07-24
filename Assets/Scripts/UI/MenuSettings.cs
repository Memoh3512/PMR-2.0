using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PMR
{
    [CreateAssetMenu(fileName = "MenuSettings", menuName = "PMR/Settings/Menu Settings")]
    public class MenuSettings : ScriptableObject
    {
        [Tooltip("This is the default menu cursor prefab that will be used for menu item selection.")]
        public GameObject DefaultMenuCursor;

        [Tooltip("This is the default menu prefab that will be used for choice menus.")]
        public GameObject DefaultChoiceMenu;

        [Tooltip("This is the default choice prefab that will be used for choice menus.")]
        public GameObject DefaultChoicePrefab;
    }
}
