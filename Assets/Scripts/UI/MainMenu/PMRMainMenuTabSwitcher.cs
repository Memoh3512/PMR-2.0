using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace PMR
{
    public class PMRMainMenuTabSwitcher : MonoBehaviour
    {
        [SerializeField] private Image checkerImage1;
        [SerializeField] private Image checkerImage2;
        
        [SerializeField] private UnityEvent<PMRMainMenuTab> OnTabSwitch;

        private PMRMainMenuTab previousTab;
        private PMRMainMenuTab nextTab;
        
        //adds the new tab to the buffer, so the anim can switch to it when ready
        public void SetNextTab(PMRMainMenuTab newTab)
        {
            previousTab = nextTab;
            nextTab = newTab;
        }

        public void ExecuteTabSwitch()
        {
            if (nextTab is null) return;

            checkerImage1.color = nextTab.backgroundColor1;
            checkerImage2.color = nextTab.backgroundColor2;
            nextTab.tabContentRoot.SetActive(true);

            if (previousTab is null) return;
            
            previousTab.tabContentRoot.SetActive(false);
        }
    }
}
