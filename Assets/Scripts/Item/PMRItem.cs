using UnityEngine;

namespace PMR.Item
{
    [CreateAssetMenu(fileName = "MyItem", menuName = "PMR/Create New Item")]
    public class PMRItem : PMRListItem
    {
        [field: SerializeField] public bool canUseOutsideCombat { get; private set; }
        
        //skeleton proposition - when ready to actually implement these, take the time to think if this is the right way
        //to implement it. It might save you a lot of time
        [SerializeField] private PMRItemLogic itemLogic;

        public void Use()
        {
            if (itemLogic == null)
            {
                Debug.LogError($"ItemLogic is null for {name} ! Please set a logic.");
            }
            
            itemLogic.OnUse();
        }
        
        public void CombatUse()
        {
            if (itemLogic == null)
            {
                Debug.LogError($"ItemLogic is null for {name}! Please set a logic.");
            }
            
            itemLogic.OnUseInCombat();
        }

        public override bool CanUse()
        {
            return GameState.IsInCombat || canUseOutsideCombat;
        }
    }

    public abstract class PMRItemLogic : ScriptableObject
    {
        public abstract void OnUse(/*maybe add a context item here*/);
        public abstract void OnUseInCombat();
    }
}