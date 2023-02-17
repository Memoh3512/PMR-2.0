using System;
using UnityEditor.Experimental.GraphView;

namespace PMR.GraphEditor.Elements
{
    public class PMRGroup : Group
    {
        [ReadOnly] public string ID;

        public PMRGroup()
        {
            ID = Guid.NewGuid().ToString();
        }
    }
}
