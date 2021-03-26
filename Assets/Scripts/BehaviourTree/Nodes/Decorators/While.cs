using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [CreateNodeMenu("BehaviourTree/Decorator/While")]
    public class While : Decorator
    {
        [HideInInspector, SerializeField]
        public string m_methodName;

        public override NodeStatus Evaluate(GameObject go)
        {
            System.Type type = go.GetType();
            System.Reflection.MethodInfo methodInfo = type.GetMethod(m_methodName);
            Debug.Assert(methodInfo != null);

            bool returnValue = (bool)methodInfo.Invoke(go, new object[] { });
            if (returnValue)
            {
                m_nodeStatus = NodeStatus.RUNNING;

                m_node.Evaluate(go);
            }
            else
            {
                m_nodeStatus = NodeStatus.SUCCESS;
            }

            return m_nodeStatus;
        }
    }
}
