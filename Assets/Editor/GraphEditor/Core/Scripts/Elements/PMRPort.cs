using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace PMR.GraphEditor.Elements
{
    public class PMRPort : Port
    {
        
        public Action<PMRPort> OnConnect { get; set; }
        
        public Action<PMRPort> OnDisconnect { get; set; }

        public PMRPort(Port port) : base(port.orientation, port.direction, port.capacity, port.portType)
        {
            m_EdgeConnector = port.edgeConnector;
            this.AddManipulator(port.edgeConnector);

        }

        public void CallOnConnect(PMRPort other)
        {
            if (other != null && OnConnect != null)
            {
                OnConnect(other);
            }
        }

        public void CallOnDisconnect(PMRPort other)
        {
            if (OnDisconnect != null)
            {
                OnDisconnect(other);
            }
        }
        
    }
}
