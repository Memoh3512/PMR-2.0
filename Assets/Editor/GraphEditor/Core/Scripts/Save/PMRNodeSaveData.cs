using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PMR.GraphEditor.Save
{
    
    using Elements;
    
    [Serializable]
    public class PMRNodeSaveData
    {
        [field: SerializeField] public string ID { get; set; }
        [field: SerializeField] public string GroupID { get; set; }
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public Vector2 Position { get; set; }

        public virtual PMRNode LoadData(PMRGraphView graphView)
        {
            throw new Exception("LoadData() Called in PMRNodeSaveData. This should not happen and should always be overridden! If you are a designer, contact a programmer.");
        }
        
    }
}
