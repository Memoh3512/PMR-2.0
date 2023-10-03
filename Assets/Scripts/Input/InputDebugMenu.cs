using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PMR
{
    public class InputDebugMenu : MonoBehaviour
    {
        [SerializeField] private InputDebugButton a, b, cup, cdown, cleft, cright, l, r, z;

        private PlayerInputActions inputActions;
        // Start is called before the first frame update
        void Start()
        {
            inputActions = new PlayerInputActions();
            inputActions.Enable();
            inputActions.Gameplay.A.started += _ => a.SetPressed(true);
            inputActions.Gameplay.A.canceled += _ => a.SetPressed(false);
            
            inputActions.Gameplay.B.started += _ => b.SetPressed(true);
            inputActions.Gameplay.B.canceled += _ => b.SetPressed(false);
            
            inputActions.Gameplay.CUp.started += _ => cup.SetPressed(true);
            inputActions.Gameplay.CUp.canceled += _ => cup.SetPressed(false);
            
            inputActions.Gameplay.CDown.started += _ => cdown.SetPressed(true);
            inputActions.Gameplay.CDown.canceled += _ => cdown.SetPressed(false);
            
            inputActions.Gameplay.CLeft.started += _ => cleft.SetPressed(true);
            inputActions.Gameplay.CLeft.canceled += _ => cleft.SetPressed(false);
            
            inputActions.Gameplay.CRight.started += _ => cright.SetPressed(true);
            inputActions.Gameplay.CRight.canceled += _ => cright.SetPressed(false);
        }
    }
}
