using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [CreateNodeMenu("BehaviourTree/Decorator/Inverter")]
    public class Inverter : Decorator
    {
        public override NodeStatus Evaluate(GameObject go)
        {
            NodeStatus status = m_node.Evaluate(go);
            if (status == NodeStatus.RUNNING)
            {
                m_nodeStatus = NodeStatus.RUNNING;
            }
            else if (status == NodeStatus.SUCCESS)
            {
                m_nodeStatus = NodeStatus.FAILURE;
            }
            else
            {
                m_nodeStatus = NodeStatus.SUCCESS;
            }

            return m_nodeStatus;
        }
    }
}
